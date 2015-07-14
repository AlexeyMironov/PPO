#include <windows.h>
#include <map>
#include <string>
#include "MeasureTime.h"
using namespace std;

__int64 g_freq; // глобальная переменная
map<string, TIME_INFORMATION> timeInformation;

void Init()
{
	LARGE_INTEGER s;
	QueryPerformanceFrequency(&s);
	g_freq = s.QuadPart;
}

__int64 GetTiсks()
{
	LARGE_INTEGER s;
	QueryPerformanceCounter(&s);
	return s.QuadPart;
}

__int64 GetMicroTickCount()		// время в mcs
{
	return (GetTiсks() * 1000000 / g_freq);
}

void MeasureTimeBegin(const char* fname)
{
	if (timeInformation.count(fname) == 0)
	{
		TIME_INFORMATION ti;
		ti.callCount = 1;
		ti.totalTime = 0;
		ti.lastCallTime = GetMicroTickCount();
		timeInformation[fname] = ti;
	}

	else
	{
		timeInformation[fname].callCount ++;
		timeInformation[fname].lastCallTime = GetMicroTickCount();
	}
}

void MeasureTimeEnd(const char* fname)
{
	timeInformation[fname].totalTime += GetMicroTickCount() - timeInformation[fname].lastCallTime;
}

void PrintResult()
{
	printf("                 function name | call count | total time | average time \n");
	printf("________________________________________________________________________\n");
	for (auto f : timeInformation)
	{
		printf("%30s | %10d | %10lld | %12lld |\n", f.first.c_str(), f.second.callCount, f.second.totalTime, f.second.totalTime / f.second.callCount);
	}
	printf("_______________________________|____________|____________|______________|\n");
}

