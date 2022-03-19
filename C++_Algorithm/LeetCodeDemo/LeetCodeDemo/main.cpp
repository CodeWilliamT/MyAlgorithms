#include "Test4.cpp"
//bittree helper
#include "0297.cpp"
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
	Codec BitTreeHelper;
	Solution s;
	vector<int> v1 = { 9,8,7,6,5,4,3,2,1,0 };
	vector<int> v2 = { 0,1,2,3,4,5,6,7,8,9 };
	vector<vector<int>> vv1 = {{0, 2, 0, 5}, {0, 1, 1, 1}, {3, 0, 3, 3}, {4, 4, 4, 4}, {2, 1, 2, 4}};
	vector<vector<int>> vv2 = {{0, 2}, {0, 3}, {0, 4}, {2, 0}, {2, 1}, {2, 2}, {2, 5}, {3, 0}, {3, 1}, {3, 3}, {3, 4}, {4, 0}, {4, 3}, {4, 5}, {5, 0}, {5, 1}, {5, 2}, {5, 4}, {5, 5}};
	vector<string> vs1 = { "ju","fzjnm","x","e","zpmcz","h","q" };
	vector<string> vs2 = { "f","hveml","cpivl","d" };
	vector < vector<string>> tuple4 = { {"d"} ,{"hveml","f","cpivl"},{"cpivl","zpmcz","h","e","fzjnm","ju"},{"cpivl","hveml","zpmcz","ju","h"},{"h","fzjnm","e","q","x"},{"d","hveml","cpivl","q","zpmcz","ju","e","x"},{"f","hveml","cpivl"} };
 	s.minimumWhiteTiles("001111110000000011111111010011110100001111000000000111111110000110000000000000001010111111111011110011111111000000010011110000111101111000111101111000111101100111100111100001111001111010000000000000000000100000100000000111100000000001100011111111011111000011111100001000000001111001111111111111011110100001111111101111101111000011110011110001111001111011111111000111100111111111111111100001111000"
		,68
		,4);
	/*string a = "())()))()(()(((())(()()))))((((()())(())";
	string b = "1011101100010001001011000000110010100101";
	s.canBeValid(a, b
	);*/
	return 0;
}