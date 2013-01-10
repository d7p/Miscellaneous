
#include "cuda_runtime.h"
#include "device_launch_parameters.h"

#include <stdio.h>
#include <time.h>
#include <types.h>

#define BIAS 0.03662654
#define NUMSUPPORTVECTORS 25

template <unsigned int blockSize>
__global__ void addKernel(double *testVector,double *supportVector1, double *supportVector2,double *alpha, double *result)
{
	//
	__shared__ double temp[NUMSUPPORTVECTORS];
    int Id = (blockIdx.y * blockDim.y + threadIdx.y)+(blockIdx.x * blockDim.x + threadIdx.x);
    temp[Id] = (alpha[Id]*(testVector[1]*supportVector1[Id])+(testVector[2]*supportVector2[Id]))+BIAS;

	//taken from the nivida slides week 3 the 7 version 
	extern __shared__ double sdata[];
	unsigned int tid = threadIdx.x;
	unsigned int i = blockIdx.x*(blockSize*2) + tid;
	unsigned int gridSize = blockSize*2*gridDim.x;
	sdata[tid] = 0;
	while (i < NUMSUPPORTVECTORS) { sdata[tid] += temp[i] + temp[i+blockSize];  i += gridSize; }
	__syncthreads();
	if (blockSize >= 512) { if (tid < 256) { sdata[tid] += sdata[tid + 256]; } __syncthreads(); }
	if (blockSize >= 256) { if (tid < 128) { sdata[tid] += sdata[tid + 128]; } __syncthreads(); }
	if (blockSize >= 128) { if (tid <  64) { sdata[tid] += sdata[tid +  64]; } __syncthreads(); }
	if (tid < 32) {
		if (blockSize >=  64) sdata[tid] += sdata[tid + 32];
		if (blockSize >=  32) sdata[tid] += sdata[tid + 16];
		if (blockSize >=  16) sdata[tid] += sdata[tid +  8];
		if (blockSize >=   8) sdata[tid] += sdata[tid +  4];
		if (blockSize >=   4) sdata[tid] += sdata[tid +  2];
		if (blockSize >=   2) sdata[tid] += sdata[tid +  1];
	}

	if (tid == 0) result[blockIdx.x] = sdata[0];
}

double SVMCPU(double testVector[2],double supportVector1[NUMSUPPORTVECTORS], double supportVector2[NUMSUPPORTVECTORS],double alpha[NUMSUPPORTVECTORS])
{
	double result=0;
	int i;
	for(i=0;i<NUMSUPPORTVECTORS;i++)
	{
	 result += (alpha[i]*(testVector[1]*supportVector1[i])+(testVector[2]*supportVector2[i]))+BIAS;
	}
	return result;
}

int main()
{
	//host variables
	//I was trying to use a 2d or more array but could not get it to work
	double testVector[2] = {1.552140,1.552510};// if this was to be used test vector should be in input to the main function (supports and alpha could also be inputs)
    double supportVector1[NUMSUPPORTVECTORS] = {1.566690,1.566060,1.566450,1.566800,1.567160,1.566520,1.566630,1.567250,1.566710,1.566750,1.566360,1.566700,1.566640,1.566920,1.566580,1.566650,1.566790,1.566780,1.566760,1.566810,1.566800,1.566820,1.566790,1.566820,1.566980};
    double supportVector2[NUMSUPPORTVECTORS] = {1.566060,1.566450,1.566800,1.567160,1.566520,1.566630,1.567250,1.566710,1.566750,1.566360,1.566700,1.566640,1.566920,1.566580,1.566650,1.566790,1.566780,1.566760,1.566810,1.566800,1.566820,1.566790,1.566820,1.566980,1.566700};
    double alpha[NUMSUPPORTVECTORS] = {-3.128623,3.209386,3.239582,3.229334,-3.123321,3.434454,3.018791,-3.205237,3.490269,-3.324280,3.248263,-3.592508,3.295397,-3.365992,3.466642,3.409259,-3.633737,-3.625539,3.481830,-3.633858,3.506001,-3.617643,3.497924,3.391959,-3.415177};
	double result[NUMSUPPORTVECTORS];
	
	//device variable pointers
	double *testVector_d;
	double *supportVector1_d;
	double *supportVector2_d;
	double *result_d;
	double *alpha_d;
	
	//Timing variables
	float gputime;
	cudaEvent_t start;
	cudaEvent_t stop;

	//Make space on the device
	cudaMalloc((void**)&supportVector1_d, NUMSUPPORTVECTORS*sizeof(double));
	cudaMalloc((void**)&supportVector2_d, NUMSUPPORTVECTORS*sizeof(double));
	cudaMalloc((void**)&result_d, NUMSUPPORTVECTORS*sizeof(double));
	cudaMalloc((void**)&testVector_d, 2*sizeof(double));
	cudaMalloc((void**)&alpha_d, NUMSUPPORTVECTORS*sizeof(double));

	//copy host variables
	cudaMemcpy(supportVector1_d, supportVector1, NUMSUPPORTVECTORS*sizeof(double), cudaMemcpyHostToDevice);
	cudaMemcpy(supportVector2_d, supportVector2, NUMSUPPORTVECTORS*sizeof(double), cudaMemcpyHostToDevice);
	cudaMemcpy(result_d, result, NUMSUPPORTVECTORS*sizeof(double), cudaMemcpyHostToDevice);
	cudaMemcpy(testVector_d, testVector, 2*sizeof(double), cudaMemcpyHostToDevice);
	cudaMemcpy(alpha_d, alpha, NUMSUPPORTVECTORS*sizeof(double), cudaMemcpyHostToDevice);
	
	//Set the dimentions
	dim3 block(1,1,1);
	dim3 grid(NUMSUPPORTVECTORS,1,1);

	//Start gpu timing
	cudaEventCreate(&start);
	cudaEventCreate(&stop);
	cudaEventRecord(start, 0);

	//Start kernel
	addKernel<32><<<grid,block>>>(testVector_d,supportVector1_d,supportVector2_d,alpha_d,result_d);

	//Copy result back
	cudaMemcpy(result, result_d, NUMSUPPORTVECTORS*sizeof(double), cudaMemcpyDeviceToHost);

	//Stop gpu timing 
	cudaEventRecord(stop, 0);
	cudaEventSynchronize(stop);
	cudaEventElapsedTime(&gputime, start, stop);
	printf("GPU elapesed time:%f\n", gputime);
	
	//free device variables
	cudaFree(testVector_d);
	cudaFree(supportVector1_d);
	cudaFree(supportVector2_d);
	cudaFree(result_d);
	cudaFree(alpha_d);

	//################CPU########################		
	//Cpu timing start
	struct timeval tp;
	double sec, usec, start, end;

	// Time stamp before the computations
	gettimeofday( &tp, NULL );
	sec = static_cast<double>( tp.tv_sec );
	usec = static_cast<double>( tp.tv_usec )/1E6;
	start = sec + usec;

	// call cpu svm
	double cpuResult = SVMCPU(testVector,supportVector1,supportVector2,alpha);
	
	//Cpu timing stop
	gettimeofday( &tp, NULL );
	sec = static_cast<double>( tp.tv_sec );
	usec = static_cast<double>( tp.tv_usec )/1E6;
	end = sec + usec;

	// Time calculation (in seconds)
	double cputime = end - start;

	//print time
	printf("CPU elapesed time:%d\n",cputime);

	//speed up
	//Scaled size speed up:s+Np
	double ScaledSpeedUp = cputime+(NUMSUPPORTVECTORS*gputime);
	printf("Scaled speed up:%d\n",ScaledSpeedUp);

	//Fixed size speed up: 1/(s+(p/N)
	double FixedSpeedUp = 1/(cputime+(guptime/NUMSUPPORTVECTORS));
	printf("Fixed speed up:%d\n",FixedSpeedUp);

    return 0;

}