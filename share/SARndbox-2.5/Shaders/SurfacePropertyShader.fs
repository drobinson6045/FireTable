
#extension GL_ARB_texture_rectangle : enable
#define PI 3.1415926535897932384626433832795
   
uniform vec2 cellSize;
uniform sampler2DRect bathymetrySampler;//NOWATER
uniform sampler2DRect fireSampler;//NOWATER


void main()
  {

    /*Central differences*/

    float dfx =texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x+1.0,gl_FragCoord.y))-texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x-1.0,gl_FragCoord.y));
    float dfy =texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x,gl_FragCoord.y+1.0))-texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x,gl_FragCoord.y-1.0));
    float az = atan(dfy/(2.0*cellSize.y),dfx/(2.0*cellSize.x));
    /*Step in azimuthal direction and get angle*/
    float dfz =texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x+cos(PI*0.0),gl_FragCoord.y+sin(PI*0.0)))-texture2DRect(bathymetrySampler, gl_FragCoord.xy);
    float alt = atan(dfz,cellSize.x);
    gl_FragColor=vec4(alt,az,dfx,0.0);


  }
