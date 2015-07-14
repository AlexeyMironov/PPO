#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <assert.h>
#include <string>
#include <vector>
#include <map>
#include "MeasureTime.h"

using namespace std;

bool line_comment = false, comment = false;
vector<string> for_map;
map<string, int> count_of_words;

void print_popular_word(){
	PROFILE_START;
	int max = -1;
	for (auto it = count_of_words.begin(); it != count_of_words.end(); it++){
		if (it->second > max){
			max = it->second;
		}
	}
	for (auto it = count_of_words.begin(); it != count_of_words.end(); it++){
		if (it->second == max){
			printf("Most popular word is : \"%s\" (%d)\n", it->first.c_str(), max);
		}
	}
	PROFILE_END;
}

void fill_map(vector<string> for_map){
	PROFILE_START;
	string word;
	for (int i = 0; i < for_map.size(); ++i)
	{
		word = "";
		for (int j = 0; j < for_map[i].length(); ++j){
			if (for_map[i][j] == ' '){
				if (word != ""){
					++count_of_words[word];
					word = "";
				}
			}
			else{
				word = word + for_map[i][j];
			}
		}
		if (word != ""){
			++count_of_words[word];
			word = "";
		}
	}
	PROFILE_END;
}

string get_str(FILE *f){
	PROFILE_START;
	char c;
	string buff;
	fscanf(f, "%c", &c);
	while ((!feof(f)) && (c != '\n')){
		buff = buff + c;
		fscanf(f, "%c", &c);
	}
	PROFILE_END;
	return buff;
}

char *delete_comment(const char *s, bool is_ut = false, bool c = false, bool lc = false){
	PROFILE_START;
	bool was_slash = false, was_star = false, in_string = false, is_changed = false;
	if (!s){
		char *res = (char*)malloc(sizeof(char));
		res[0] = '\0';
		PROFILE_END;
		return res;
	}
	string buff;
	char *res = (char*)malloc(strlen(s) + 1);
	int len = strlen(s), k = 0, i = 0;
	const char **cur = &s;
	while (((*cur + k) - s < len) && !lc){
		if (*(*cur + k) == '/' && !in_string){
			if (was_slash && !c){
				c = true;
				lc = true;
				comment = true;
				line_comment = true;
				is_changed = true;
			}
			else{
				if (was_star && c){
					c = false;
					comment = false;
					is_changed = true;
				}
			}
			was_slash = true;
		}
		else{
			if (*(*cur + k) == '*' && !in_string){
				if (!c && was_slash){
					c = true;
					lc = false;
					comment = true;
					is_changed = true;
					line_comment = false;
				}
				was_star = true;
			}
			else
			{
				if (*(*cur + k) == '"'){
					if (in_string)
						in_string = false;
					else
					if (!c)
						in_string = true;
				}
				if (was_slash && !c && !is_changed){
					res[i] = '/';
					i++;
				}
				was_slash = false;
				was_star = false;
			}
		}
		if (!c && !was_slash){
			res[i] = *(*cur + k);
			i++;
		}
		if (c && !lc){
			if (*(*cur + k) != '*')
				buff = buff + *(*cur + k);
		}
		k++;
	}

	if (lc){
		while ((*cur + k) - s < len){
			buff = buff + *(*cur + k);
			++k;
		}
	}
	res[i] = '\0';

	if (!buff.empty() && !is_ut){
		for_map.push_back(buff);
	}
	PROFILE_END;
	return res;
}

void unit_test_for_delete_comment(){
	PROFILE_START;
	char **tests = (char**)malloc(9 * sizeof(char*));
	tests[0] = delete_comment("normal string", true);
	tests[1] = delete_comment("line comment here ===>//try", true);
	tests[2] = delete_comment("//must show nothing", true);
	tests[3] = delete_comment("comment===>/*jgbfkjg*/", true);
	tests[4] = delete_comment("/*FIFtH*/<===comment", true);
	tests[5] = delete_comment("comment with lcomment inside===>/*jnfjgfgkm//nkgmfklg*/", true);
	tests[6] = delete_comment("\"string(lc must be here)===>//GG WP\"jkkk", true, false, false);
	tests[7] = delete_comment("", true);
	tests[8] = delete_comment("lc===>//j/*j*/q/*jj*/", true);

	assert(!strcmp(tests[0], "normal string"));
	free(tests[0]);
	assert(!strcmp(tests[1], "line comment here ===>"));
	free(tests[1]);
	
	assert(!strcmp(tests[2], ""));
	free(tests[2]);
	assert(!strcmp(tests[3], "comment===>"));
	free(tests[3]);
	assert(!strcmp(tests[4], "<===comment"));
	free(tests[4]);
	assert(!strcmp(tests[5], "comment with lcomment inside===>"));
	free(tests[5]);
	assert(!strcmp(tests[6], "\"string(lc must be here)===>//GG WP\"jkkk"));
	free(tests[6]);
	assert(!strcmp(tests[7], ""));
	free(tests[7]);
	assert(!strcmp(tests[8], "lc===>"));
	free(tests[8]);
	free(tests);
	PROFILE_END;
}

int main(int argc, char *argv[]){
	Init();
	PROFILE_START;
	unit_test_for_delete_comment();

	if (argc < 3) printf("Not enought parametrs (expected <input_filename> <output_filename>)\n");
	else {
		FILE *fi, *fo;
		if ((fi = fopen(argv[1], "r")) == 0){
			printf("Can not open file %s", argv[1]);
			getchar();
			PROFILE_END;
			return 1;
		}
		if ((fo = fopen(argv[2], "w")) == 0){
			printf("Can not create file %s", argv[2]);
			getchar();
			PROFILE_END;
			return 1;
		}
		else{
			comment = false;
			line_comment = false;
			vector<string> strings;
			while (!feof(fi)){
				strings.push_back(get_str(fi));
			}
			for (auto it = strings.begin(); it != strings.end(); it++)
			{
				if (comment && line_comment)
				{
					comment = false;
					line_comment = false;
				}

				string res = delete_comment(it->c_str(), false, comment, line_comment);
				fprintf(fo, "%s", res.c_str());
				if (!(comment && !line_comment))
					fprintf(fo, "\n");
			}
		}

		fclose(fo);
		fclose(fi);
	}
	fill_map(for_map);
	print_popular_word();
	PROFILE_END;
	PrintResult();
	system("pause");
	return 0;
}