#define _CRT_SECURE_NO_WARNINGS
using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
#include "myAlgo\LCParse\LCParse.cpp"
//#include "myAlgo\LCParse\TreeNode.cpp"
//#include "2569.cpp"
#include "Test3.cpp"
//#include "0297.cpp"
//bittree helper
/*
struct TreeNode {
	int val;
	TreeNode* left;
	TreeNode* right;
	TreeNode() : val(0), left(nullptr), right(nullptr) {}
	TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
	TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};
*/


/*
struct ListNode {
	int val;
	ListNode* next;
	ListNode() : val(0), next(nullptr) {}
	ListNode(int x) : val(x), next(nullptr) {}
	ListNode(int x, ListNode* next) : val(x), next(next) {}
};
*/
/*
class Node {
public:
	int val;
	Node* left;
	Node* right;
	Node* next;
	Node() : val(0), left(NULL), right(NULL), next(NULL) {}
	Node(int _val) : val(_val), left(NULL), right(NULL), next(NULL) {}
	Node(int _val, Node* _left, Node* _right, Node* _next)
		: val(_val), left(_left), right(_right), next(_next) {}
};
*/
int main()
{
	//Codec BitTreeHelper;
	Solution s;
	string sin[10];
	string tmp;
	int num[10];
	string str[10];
	vector<int> v[10];
	vector<string> vs[10];
	vector<string> vs2 = { "not" };
	vector<string> vs3 = { "this student is not studious","the student is smart" };
	vector<vector<int>> vvi[10];
	vector<vector<int>> vvi2 = { {0,1}, {1, 0}};
	vector<vector<char>> vvc[10];
	vector<vector<string>> vvs[10];
	//s.handleQuery(v1,v2,vvi1);
	TreeNode* node[10];
	while (true) {
		for (int i = 0; i < 2; i++) {
			std::getline(cin, sin[i]);
			if(cin.fail()||sin[i].empty())
				break;
			if (sin[i][0] != '[')
				num[i] = stoi(sin[i]);
			if (sin[i].size() <2){
				continue;
			}
			str[i] = sin[i].substr(1, sin[i].size() - 2);
			v[i] = stov(sin[i]);
			vs[i]=stovs(sin[i]);
			if (sin[i][1]!='[') {
				continue;
			}
			vvi[i] = stovvi(sin[i]);
			vvc[i] = stovvc(sin[i]);
			vvs[i] = stovvs(sin[i]);
			node[i]=stotree(sin[i]);
		}
		if (cin.fail())
			break;
		auto rst=s.lengthOfLongestSubsequence(v[0], num[1]);
		//string srst=treetos(rst);
		cout << rst << endl << endl;
	}

	//string name, url;
	////将标准输入流重定向到 in.txt 文件
	//freopen("in.txt", "r", stdin);
	//cin >> name >> url;
	////将标准输出重定向到 out.txt文件
	//freopen("out.txt", "w", stdout);
	//cout << name << "\n" << url;
	/*string a = "())()))()(()(((())(()()))))((((()())(())";
	string b = "1011101100010001001011000000110010100101";
	s.canBeValid(a, b
	);*/
	return 0;
}