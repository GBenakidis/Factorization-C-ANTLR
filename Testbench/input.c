#include <stdio.h>
#include <stdlib.h>
float add(float a,float b);
float sub(float a,float b);
void main(int argc, char* argv[]){
	//  ***** Local declarations *****
	float k;
	//  ***** Code Body *****
	add(2,3);
	sub(2,3);
	
}
float add(float a,float b){
	//  ***** Local declarations *****
	//  ***** Code Body *****
	k = 0;
	printf("k = %f\n", k);
	return a + b;
	
}
float sub(float a,float b){
	//  ***** Local declarations *****
	//  ***** Code Body *****
	return a - b;
	
}

