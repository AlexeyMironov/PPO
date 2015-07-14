#pragma once
#define PROFILE_START MeasureTimeBegin(__FUNCTION__)
#define PROFILE_END MeasureTimeEnd(__FUNCTION__)

void MeasureTimeBegin(const char* fname);
void MeasureTimeEnd(const char* fname);
void PrintResult();

typedef struct
{
	int callCount;
	__int64 totalTime;
	__int64 lastCallTime;
}TIME_INFORMATION;

void Init();
__int64 GetMicroTickCount();