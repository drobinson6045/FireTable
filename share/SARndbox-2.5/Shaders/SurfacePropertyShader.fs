
#extension GL_ARB_texture_rectangle : enable
#define PI 3.1415926535897932384626433832795
   
uniform vec2 cellSize;
uniform sampler2DRect bathymetrySampler;//NOWATER
uniform sampler2DRect fireSampler;//NOWATER


void main()
  {

    /*Central differences*/
    /*
    float dfx =texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x+1.0,gl_FragCoord.y))-texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x-1.0,gl_FragCoord.y));
    float dfy =texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x,gl_FragCoord.y+1.0))-texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x,gl_FragCoord.y-1.0));/*
    float dx = cellSize.x;
    float dy = cellSize.y;
    float avY=0, avX=0;
    //Calculate average height
    for(int i=0; i<5; i++){
      avX +=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x-2.0+i*1.0,gl_FragCoord.y))/5.0;
      avY +=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x,gl_FragCoord.y-2.0+i*1.0))/5.0;
    }
    float numX = 0.0;
    float numY = 0.0;
    float x1=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x-2.0,gl_FragCoord.y));
    float x2=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x-1.0,gl_FragCoord.y));
    float x3=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x+1.0,gl_FragCoord.y));
    float x4=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x+2.0,gl_FragCoord.y));
    
    float y1=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x,gl_FragCoord.y-2.0));
    float y2=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x,gl_FragCoord.y-1.0));
    float y3=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x,gl_FragCoord.y+1.0));
    float y4=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x,gl_FragCoord.y+2.0));
    float slopeX = 2.0*x1 + x2 + x3 +2.0*x4 - 6.0*avX;
    float slopeY = 2.0*x1 + x2 + x3 +2.0*x4 - 6.0*avX;
    slopeX = slopeX/(10.0*dx);
    slopeY = slopeY/(10.0*dy);
    

    //using least squares
    float az = atan(dfy/(2.0*cellSize.y),dfx/(2.0*cellSize.x));
    //Step in azimuthal direction and get angle
    float dfz =texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x+cos(PI*0.5),gl_FragCoord.y+sin(PI*0.5)))-texture2DRect(bathymetrySampler, gl_FragCoord.xy);
    float alt = atan(dfz,cellSize.x);
    float elev = texture2DRect(bathymetrySampler, gl_FragCoord.xy).r;
    gl_FragColor=vec4(slopeX,slopeY,elev,0.0);//(alt,az,elev)


  }
