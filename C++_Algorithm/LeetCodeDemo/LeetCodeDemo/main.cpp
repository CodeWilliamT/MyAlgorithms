#include "header.h"
#include "0918.cpp"
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
struct ListNode {
	int val;
	ListNode* next;
	ListNode() : val(0), next(nullptr) {}
	ListNode(int x) : val(x), next(nullptr) {}
	ListNode(int x, ListNode* next) : val(x), next(next) {}
};
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
	string str = "000009";
	vector<int> v1 = {-3,-2,-3};
	vector<int> v2 = { 0,1,2,3,4,5,6,7,8,9 };
	vector<vector<int>> vvi1 = { {2,1,3},{6,5,4},{7,8,9} };
	vector<vector<int>> vvi2 = {{2, 1}, {1, 2}, {0, 1}, {1, 0}};
	vector<vector<char>> vvc = {{'(', '(', '('}, {')', '(', ')'}, {'(', '(', ')'}, {'(', '(', ')'}};
	vector<string> vs1 = { "ju","fzjnm","x","e","zpmcz","h","q" };
	vector<string> vs2 = { "f","hveml","cpivl","d" };
	vector < vector<string>> tuple4 = { {"d"} ,{"hveml","f","cpivl"},{"cpivl","zpmcz","h","e","fzjnm","ju"},{"cpivl","hveml","zpmcz","ju","h"},{"h","fzjnm","e","q","x"},{"d","hveml","cpivl","q","zpmcz","ju","e","x"},{"f","hveml","cpivl"} };
	
	s.maxSubarraySumCircular(v1);
	//���
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

	/*string a = "())()))()(()(((())(()()))))((((()())(())";
	string b = "1011101100010001001011000000110010100101";
	s.canBeValid(a, b
	);*/
	return 0;
}