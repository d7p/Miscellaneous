//********************************************************************//
//                                                                    //
// Class to implement a simple homogeneous coordinate transformation  //
// matrix.                                                            //
//                                                                    //
// This code and the class structures were written by me (Nigel       //
// Barlow)(nigel@soc.plymouth.ac.uk).   You may use this code for any //
// educational or non-profit making purpose, so long as you leave my  //
// name on it.  And please cirrext some of the typinf and spellig     //
// errirs.    Nigel Barlow <nigel@soc.plymouth.ac.uk>                 //
//                                                                    //
//********************************************************************//


using System;
using System.Collections.Generic;
using System.Text;


namespace The_Famous_Perspective_Cube
{
    class Nmatrix
    {

        
//********************************************************************//
//                                                                    //
// Constructors.                                                      //
//                                                                    //
//********************************************************************//

    public Nmatrix()
      {
      initAsIdentityMatrix();
      }


     public Nmatrix(String name)
       {
        initAsIdentityMatrix();
        matrixName = name;
       }

//********************************************************************//
//                                                                    //
// Class variables.                                                   //
//                                                                    //
//********************************************************************//

    public double[, ] matrix = new double[4, 4];
    public String matrixName = "Matrix";
  

//********************************************************************//
//                                                                    //
// Initialise as various types of matrix...                           //
//                                                                    //
//********************************************************************//

     public void initAsIdentityMatrix()
        {
        for (int row = 0; row < 4; row++)
            for (int col = 0; col < 4; col++) matrix[col, row] = 0;

        matrix[0, 0] = 1;           matrix[1, 1] = 1;  
        matrix[2, 2] = 1;           matrix[3, 3] = 1;
    }


//********************************************************************//

  public void initAsTranslationMatrix(double tx, double ty, double tz)
    {
    initAsIdentityMatrix();
    matrix[0, 0] = 1;         //Now all we need do is insert//
    matrix[1, 1] = 1;         //the non-zeros.              //
    matrix[2, 2] = 1;
    matrix[3, 3] = 1;
    matrix[0, 3] = tx;  matrix[1, 3] = ty;    matrix[2, 3] = tz;
    }


//********************************************************************//

public void initAsScalingMatrix(double sx, double sy, double sz)
    {
    initAsIdentityMatrix();
    matrix[0, 0] = sx;         //Now all we need do is insert//
    matrix[1, 1] = sy;         //the non-zeros.              //
    matrix[2, 2] = sz;
    matrix[3, 3] = 1;          
    }


//********************************************************************//

  public void initAsPerspectiveMatrix(double f)
    {
    initAsIdentityMatrix();
    matrix[3, 2] = 1/f;
    matrix[0, 0] = 1;    matrix[1, 1] = 1;    matrix[2, 2] = 1;
    matrix[3, 3] = 0;
    }


//********************************************************************//
// You need create some more, such as                                 //
//                                                                    //
// public void initAsRotateXmatrix(double rx)                         //
// public void initAsRotateYmatrix(double ry)                         //
//...and more.                                                        //
//                                                                    //
// ...and multiply by another matrix...                               //
//                                                                    //
//  public Nmatrix multiply(Nmatrix matIn)                            //
//********************************************************************//


    
    
    
    }
}
