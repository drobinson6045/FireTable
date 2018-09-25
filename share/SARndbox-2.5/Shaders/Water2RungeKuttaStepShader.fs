/***********************************************************************
Water2RungeKuttaStepShader - Shader to perform a Runge-Kutta integration
step.
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

uniform sampler2DRect bathymetrySampler;//NOWATER
uniform sampler2DRect fireSampler;//NOWATER

void main()
	{
	/* Calculate the Runge-Kutta step: */
	float sHeight=texture2DRect(bathymetrySampler,gl_FragCoord.xy).r;
	
	//vec3 qStar=texture2DRect(quantityStarSampler,gl_FragCoord.xy).rgb;
	//vec3 qt=texture2DRect(derivativeSampler,gl_FragCoord.xy).rgb;
	vec3 fire=texture2DRect(fireSampler,gl_FragCoord.xy).rgb;
	//vec3 newQ=qStar;//NOWATER(q+qStar+qt*stepSize*0.0)*0.5;
	//NOWATERnewQ.yz*=attenuation;
	
	gl_FragColor=vec4(sHeight,0.0,0.0,0.0);

	}
