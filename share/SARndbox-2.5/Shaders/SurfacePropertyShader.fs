
#extension GL_ARB_texture_rectangle : enable
#define PI 3.1415926535897932384626433832795
   
uniform vec2 cellSize;
uniform sampler2DRect bathymetrySampler;//NOWATER
uniform sampler2DRect fireSampler;//NOWATER


void main()
  {
    gl_FragColor=vec4(0.0,0.0,0.0,0.0);


  }
