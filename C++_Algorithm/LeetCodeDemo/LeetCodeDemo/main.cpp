#define _CRT_SECURE_NO_WARNINGS
#include "header.h"
//#include "2569.cpp"
#include "2337.cpp"
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
	string str1 = "R_L_";
	string str2 = "__LR";
	vector<int> v1 = { 2,1,3,2,1 };
	vector<int> v2 = { 3,2,5 };
	vector<vector<int>> vvi1 = { {0,0,0,1},{0,0,0,0 }, {0, 0, 0, 0}, {1, 0, 0, 0} };
	vector<vector<int>> vvi2 = { {0,0,1}, {0, 0,0},{0,0,0} };
	vector<vector<char>> vvc = { {'(', '(', '('}, {')', '(', ')'}, {'(', '(', ')'}, {'(', '(', ')'} };
	vector<string> vs1 = { "A..","AAA","..." };
	vector<string> vs2 = { "eee", "ffff" };
	vector < vector<string>> tuple4 = { {"d"} ,{"hveml","f","cpivl"},{"cpivl","zpmcz","h","e","fzjnm","ju"},{"cpivl","hveml","zpmcz","ju","h"},{"h","fzjnm","e","q","x"},{"d","hveml","cpivl","q","zpmcz","ju","e","x"},{"f","hveml","cpivl"} };

	//s.handleQuery(v1,v2,vvi1);
	s.canChange(str1,str2);
	//
	//ofstream dataFile;
	//dataFile.open("dataFile.txt", std::ios::out | std::ios::app);
	//int output=0,tmp;
	//dataFile << "{";
	//for (int i = 1; i <= 2e9; i++) {
	//	tmp=s.countSpecialNumbers(i);
	//	if (output == tmp)continue;
	//	output = tmp;
	//	dataFile<<",{" << i << ',' << output<<"}";// д������
	//	if (i % 10==0)dataFile << endl;
	//}
	//dataFile << "}";
	//dataFile.close();// �ر��ĵ�

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