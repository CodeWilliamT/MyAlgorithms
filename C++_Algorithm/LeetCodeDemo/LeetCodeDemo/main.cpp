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
//#include "myAlgo\Structs\TreeNode.cpp"
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
	string str1 = "[1,null,2,null,3,5,4,6,null,9,8,null,7]";
	string str2 = "__LR";
	vector<int> v1 = { 3,2,5,5,2,3 };
	vector<int> v2 = { 3,2,5 };
	vector<vector<int>> vvi1 = { {1,0}};
	vector<vector<int>> vvi2 = { {0,1}, {1, 0}};
	vector<vector<char>> vvc = { {'(', '(', '('}, {')', '(', ')'}, {'(', '(', ')'}, {'(', '(', ')'} };
	vector<string> vs1 = { "A..","AAA","..." };
	vector<string> vs2 = { "eee", "ffff" };
	vector < vector<string>> tuple4 = { {"d"} ,{"hveml","f","cpivl"},{"cpivl","zpmcz","h","e","fzjnm","ju"},{"cpivl","hveml","zpmcz","ju","h"},{"h","fzjnm","e","q","x"},{"d","hveml","cpivl","q","zpmcz","ju","e","x"},{"f","hveml","cpivl"} };

	//s.handleQuery(v1,v2,vvi1);
	/*Codec codec;
	TreeNode* node= codec.deserialize(str1);
	string testStr=codec.serialize(node);*/
	s.maximumSumOfHeights(v1);

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