/***********************************************************************
Water2SlopeAndFluxAndDerivativeShader - Shader to compute the temporal
derivative of the conserved quantities directly from spatial partial
derivatives, bypassing the separate partial flux computation.
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
#extension GL_ARB_draw_buffers : enable
#define PI 3.1415926535897932384626433832795

uniform vec2 cellSize;
uniform float theta;
uniform float g;
uniform float epsilon;
uniform sampler2DRect bathymetrySampler;
uniform sampler2DRect quantitySampler;


void main()
	{
	/* Calculate bathymetry slope(radians) and angle of max slope(radians): */
	float br=texture2DRect(bathymetrySampler,vec2(gl_FragCoord.x+cellSize.x,gl_FragCoord.y)).r;
	float bl=texture2DRect(bathymetrySampler,vec2(gl_FragCoord.x-cellSize.x,gl_FragCoord.y)).r;
	float bu=texture2DRect(bathymetrySampler,vec2(gl_FragCoord.x,gl_FragCoord.y+cellSize.y)).r;
	float bd=texture2DRect(bathymetrySampler,gl_FragCoord.x,gl_FragCoord.y-cellSize.y).r;
        /* Calculate central differences */
        float slopeX = (br-bl)/(2*cellSize.x);
        float slopeY = (bu-bd)/(2*cellSize.y);
        float az = atan(slopeY/slopeX);
        if(az <0){ az = 2.0*PI + az;}  //atan returns [-PI,PI]
        //Now calculate slope angle in that direction
	float bp=texture2DRect(bathymetrySampler,vec2(gl_FragCoord.x+cellSize.x*cos(az),gl_FragCoord.y+cellSize.x*sin(az))).r;
	float bc=texture2DRect(bathymetrySampler,gl_FragCoord.xy).r;
        float slope = atan((bp-bc)/cellSize.x); //should use better geometry practices here
        
	
	
	/* Calculate partial fluxes across the cell's faces and the maximum possible step size for this cell: */
	gl_FragData[1]=vec4(0.0, 0.0,0.0,0.0);
	
	
	/* Set derivative texture values*/
	gl_FragData[0]=vec4(slope,az,0.0,0.0);
	}
