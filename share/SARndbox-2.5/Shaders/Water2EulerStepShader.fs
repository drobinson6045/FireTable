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
uniform sampler2DRect bathymetrySampler;

void main()
	{
	/* Calculate the Euler step: */
	//if(gl_FragCoord.x>=1 && gl_FragCoord.y>=1){
	//vec3 elevations = {0.0, 1000.0,  1000.0};
	//vec3 q=texture2DRect(quantitySampler,vec2(gl_FragCoord.x-0.051,gl_FragCoord.y-0.051)).rgb;
	/*}/*else{
                vec3 q=texture2DRect(quantitySampler,gl_FragCoord.xy).rgb;
	}
	*/
        float directions[9] = {3.0*PI*0.25, PI*0.5, PI*0.25, PI, 0.0, 0.0, 3.0*PI*0.25, 3.0*PI*0.5, 7.0*PI*0.25};
        float shiftX[9] = {-0.51, 0.0, 0.51, -0.51, 0.0, 0.51, -0.51, 0.0, 0.51}; 
        float shiftY[9] = {0.51,0.51,0.51,0.0,0.0,0.0,-0.51,-0.51,-0.51}; 
        float totFire = 0.0;
        for(int i=0; i<9; i++){
          vec3 quant = texture2DRect(quantitySampler,vec2(gl_FragCoord.x+shiftX[i],gl_FragCoord.y+shiftY[i])).rgb;
	  vec3 bath = texture2DRect(bathymetrySampler,vec2(gl_FragCoord.x+shiftX[i],gl_FragCoord.y+shiftY[i])).rgb;
          float diff = quant.r-bath.r;
          if(diff >=0.0 && diff<= 3.0){
            totFire += diff;
          }
        }
        vec3 curQuant = texture2DRect(quantitySampler,gl_FragCoord.xy).rgb;        
        curQuant.r += totFire;
	//vec3 qt=texture2DRect(derivativeSampler,gl_FragCoord.xy).rgb;
	//vec3 newQ=q+qt*stepSize*0.0;
	newQ.yz*=attenuation;
	//if(bath.r >= 9.0 ){
        //		gl_FragColor=vec4(bath+elevations,0.0);
	//}
	//else{
	//	gl_FragColor=vec4(10.0,0.0,0.0,0.0);
	//}
        
        gl_FragColor = vec4(curQuant,0.0);
	
	}
