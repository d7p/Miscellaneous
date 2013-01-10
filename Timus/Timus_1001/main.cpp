#include <iostream>
#include <math.h>

using namespace std;

int main()
{
    float d;
    float output[1024/sizeof(float)]; //(262144/sizeof(float))];
    int index=0,tmp = 0;
    do{
        scanf("%f",&d);
        output[index] = sqrt(d);
        index++;
        tmp += sizeof(float);

    }while(tmp<1024);//262144);

    while(index>0)
    {
        printf("%f\n",output[index]);
        index--;
    }

    return 0;
}

