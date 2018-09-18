/***********************************************************************
Water2EulerStepShader - Shader to perform an Euler integration step.
Copyright (c) 2012 Oliver Kreylos

This file is part of the Augmented Reality Sandbox (SARndbox).

The Augmented Reality Sandbox is free software; you can redistribute it
and/or modify it under the terms of the GNU General Public License as
published by the Free Software Foundation; either version 2 of the
License, or (at your option) any later version.

The Augmented Reality Sandbox is distributed in the hope that it will be
useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
General Public License for more details.

You should have received a copy of the GNU General Public License along
with the Augmented Reality Sandbox; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
***********************************************************************/

#extension GL_ARB_texture_rectangle : enable
#define PI 3.1415926535897932384626433832795
   
uniform float stepSize;
uniform float attenuation;
uniform sampler2DRect quantitySampler;
uniform sampler2DRect derivativeSampler;
uniform sampler2DRect bathymetrySampler;//NOWATER
uniform sampler2DRect fireSampler;//NOWATER
uniform vec2 cellSize;

void main()
	{
        /*Calculate phiS, phiW, and EBar
            phiS = 5.275*Beta^(-0.3)*tan(phi^2)
            phiW = C*(0.3048U)^B*(beta/betaOpt)^(-E)
            -requires C,B, and E
              C = 7.47e^(-0.8711*sigma^0.55)
              B = 0.15988*sigma^0.54
              E = 0.715*e^-0.01094*sigma
            EBar = sqrt[1-(LW)^(-2)]
            LW = 0.936*e^(50.5*Ueq)+0.461*e^(-30.5*Ueq)
        */  
	/* Calculate the Euler step: */
        /* Matrices for directions (0->2PI)*/
        float directions[8] = {0.0,PI*0.25,PI*0.5,3.0*PI*0.25,PI,0.0,5.0*PI*0.25,3.0*PI*0.5,7.0*PI*0.25};
        float shiftX[8] = {0.51,0.51,0.0,-0.51,-0.51,-0.51,0.0,0.51}; 
        float shiftY[8] = {0.0,0.51,0.51,0.51,0.0,-0.51,-0.51,-0.51}; 
        float dd = sqrt(2.0)*cellSize.x;//diagonal grid distance to next cell center
        float distances[2] = {cellSize.x,dd};
        float totFire = 0.0;
        //Parameters:
        float tb = 4.0; //burn-out time
        float wD = PI*0.25; //Wind-Direction with respect to x-axis
        float wS = 3.0; //Wind-Speed (m/s)  might need to be cm/s
        float beta = 1.5; //values: 1.12(litter) 0.12(grass) 0.19(shrub) 
        float sigma = 8000.0; //values: 5258(litter) 12781(grass) 6307(shrub)
        float R0 = 1.0;  //max spread rate on flat land
        //Calculate constants
        float C = 7.47*exp(-0.8711*pow(sigma,0.55));
        float B = 0.15988*pow(sigma,0.54);
        float E = 0.715*exp(-0.01094*sigma);
        float LW = 0.936*exp(50.5*wS)+0.461*exp(-30.5*wS);
        float EBar = sqrt(1.0-pow(LW,-2.0));
        float phiW = C*pow(0.3048*wS,B)*pow(beta,-E); //Constant for now  [m/s]
        float cont = 0.0;
        float cTime = 10.0;
        //Get current cell quantities for fire
        vec3 curFire = texture2DRect(fireSampler,gl_FragCoord.xy)).rgb;
        if(curFire.g < tb)
        {
          for(int i=0; i<8; i++)
          {
            // fire = <currentFquantity, burningTime, maxTimestepSize, 0.0>

            vec3 fire = texture2DRect(fireSampler,vec2(gl_FragCoord.x+shiftX[i],gl_FragCoord.y+shiftY[i])).rgb;
            vec3 groundState = texture2DRect(derivativeSampler,vec2(gl_FragCoord.x+shiftX[i],gl_FragCoord.y+shiftY[i])).rgb;
            if(fire.r>0) //Not going to burnout and burning  Need to handle burnout
            {
              //Calctheta
              float theta = groundState.g - directions[i];  //simple case groundState.g -> wD
              float phiS = 5.275*pow(beta,-0.3)*tan(pow(groundState.r,2));
              float eR = R0*(1.0+phiS+phiW);
              float spread = eR*(1.0 - EBar)/(1.0-EBar*cos(theta));
              cont += spread*stepSize/distances[i%2];
              float maxtime = distances[i%2]/spread;
              if(cTime > maxtime){cTime = maxTime;}//Keep track of timestep constraint 
            }
          }
          float updatedTime = curFire.g + stepSize;
          if(updatedTime >= tb)
          {
            gl_FragColor = vec4(-1.0,updatedTime,cTime,0.0);
          }else
          {
            gl_FragColor = vec4(cont,updatedTime,cTime,0.0);
          }
        }else{   //Leave frag alone
          gl_FragColor = vec4(curFire,0.0);
        }

      }
