
#extension GL_ARB_texture_rectangle : enable
#define PI 3.1415926535897932384626433832795
   
uniform vec2 cellSize;
uniform sampler2DRect bathymetrySampler;//NOWATER
uniform sampler2DRect fireSampler;//NOWATER


void main()
  {

    /*Central differences*/
    
    float dfx =texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x+1.0,gl_FragCoord.y)).r-texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x-1.0,gl_FragCoord.y)).r;
    float dfy =texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x,gl_FragCoord.y+1.0)).r-texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x,gl_FragCoord.y-1.0)).r;
    float dx = cellSize.x;
    float dy = cellSize.y;
    float avY=0, avX=0;
    //Calculate average height
    for(int i=0; i<5; i++){
      avX +=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x-2.0+i*1.0,gl_FragCoord.y)).r/5.0;
      avY +=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x,gl_FragCoord.y-2.0+i*1.0)).r/5.0;
    }
    float numX = 0.0;
    float numY = 0.0;
    float x1=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x-2.0,gl_FragCoord.y)).r;
    float x2=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x-1.0,gl_FragCoord.y)).r;
    float x3=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x+1.0,gl_FragCoord.y)).r;
    float x4=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x+2.0,gl_FragCoord.y)).r;
    
    float y1=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x,gl_FragCoord.y-2.0)).r;
    float y2=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x,gl_FragCoord.y-1.0)).r;
    float y3=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x,gl_FragCoord.y+1.0)).r;
    float y4=texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x,gl_FragCoord.y+2.0)).r;
    //float slopeX = 2.0*x1 + x2 + x3 +2.0*x4 - 6.0*avX;
    //float slopeY = 2.0*y1 + y2 + y3 +2.0*y4 - 6.0*avY;
    float slopeX = (x3 - x2)/(2.0*dx);
    float slopeY = (y3 - y2)/(2.0*dy); 
    //float slopeX = -(-x1+8.0*x2-8.0*x3+x4)/(12.0*dx);
    //float slopeY = -(-y1+8.0*y2-8.0*y3+y4)/(12.0*dy);

    float angleX = atan(slopeX,1.0);
    float angleY = atan(slopeY,1.0);
    //slopeX = atan(slopeX,1.0);
    //slopeY = atan(slopeY,1.0);
    

    
    float az = atan(slopeY,slopeX);
    //Step in azimuthal direction and get angle
    float dfz =texture2DRect(bathymetrySampler, vec2(gl_FragCoord.x+dx*cos(az),gl_FragCoord.y+dy*sin(az)))-texture2DRect(bathymetrySampler, gl_FragCoord.xy);
    float length = pow(pow(dx*cos(az),2.0)+pow(dy*sin(az),2.0),0.5);
    float alt = dfz/length;//tan(theta)
    float elev = texture2DRect(bathymetrySampler, gl_FragCoord.xy).r;
    gl_FragColor=vec4(alt,az,elev,0.0);//(alt,az,elev)


  }
