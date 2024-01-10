using namespace std;
#include <vector>
#include <string>
#include <unordered_map>
#include <functional>

//ǰ׺��Trie Tree
//���ã�����l���ַ������ҵ� ��һ�� ��n���ַ���ƽ������Ϊm���ַ��������� ��ƥ����ַ���ǰ׺��
//tip:������ַ�����ֱ�����ֵ����鹹���ַ�����ϣȥ�飬���졣
//O(n*m+l) O(n*m)
class Trie {
	struct TrieNode {
		unordered_map<char, TrieNode*> c;
		string w = "";
	};
	TrieNode* root = new TrieNode();
public:
	Trie() {
	}
	//��ǰ׺���в����ַ��� word
	void insert(string w) {
		TrieNode* cur = root;
		for (char& c : w) {//ÿ���ַ�ʱ������ַ���Ӧ�ڵ�����������ýڵ㣬�������򴴽�
			if (!cur->c.count(c)) {
				cur->c[c] = new TrieNode();
			}
			cur = cur->c[c];
		}
		cur->w = w;//����ÿ�����ʵ��ַ�������洢�õ���
	}
	//��ѯ�ַ����Ƿ���֮ǰ�������
	bool search(string word) {
		TrieNode* cur = root;
		for (int i = 0; i < word.size(); i++) {
			if (!cur->c.count(word[i]))
				return false;
			cur = cur->c[word[i]];
		}
		return cur->w != "";
	}
	//��ѯ�ַ����Ƿ���֮ǰ��������ַ���ǰ׺��
	bool startsWith(string prefix) {
		TrieNode* cur = root;
		for (int i = 0; i < prefix.size(); i++) {
			if (!cur->c.count(prefix[i]))
				return false;
			cur = cur->c[prefix[i]];
		}
		return true;
	}
};

//����һ�����ݼ�������˼ά
//ǰ׺��Trie + ����
//����ǰ׺�� :
//������ѯ������ÿ������.
//ÿ���ַ�ʱ������ַ���Ӧ�ڵ�����������ýڵ㣬�������򴴽�.
//����ÿ�����ʵ��ַ�������洢�õ��ʡ�
//����̽����
//����ʱ���ж�ǰ׺���ڵ��Ƿ���ڵ��ʣ�
//��֦��ɾ��ÿ��Ҷ���㣨������childs�Ľڵ㣩������(����̽����);
//���ظ���Ĩ����̽��·���ĵ㣬��̽���ظ���
//������̽ʱ����̽���ַ������ҺϷ����������һ�λ��ݣ�
class TrieTreeOnMap {
	vector<string> ans;
	struct TrieNode{
		unordered_map<char, TrieNode*> c;
		string w  = "";
	};
public:
	//���ַ���άͼ�����ַ��������б��ڵĵ��ʡ�
	vector<string> findWords(vector<vector<char>>& b, vector<string>& ws) {
		ans.clear();
		TrieNode* root  = new TrieNode();
		//1.����ǰ׺��
		for (auto& w : ws) {//������ѯ������ÿ�����ʣ�
			TrieNode* cur = root;
			for (char& c : w) {//ÿ���ַ�ʱ������ַ���Ӧ�ڵ�����������ýڵ㣬�������򴴽�
				if (!cur->c.count(c)) {
					cur->c[c] = new TrieNode();
				}
				cur = cur->c[c];
			}
			cur->w = w;//����ÿ�����ʵ��ַ�������洢�õ���
		}
		//2.���������򣬴����ڸ��ڵ��ϵ��ַ��ڵ㿪ʼ���л��ݱȶ�
		for (int i  = 0; i  < b.size(); i++) {
			for (int j  = 0; j  < b[i].size(); j++) {
				if (root->c.count(b[i][j]))
					backTrack(i,j,root,b);
			}
		}
		return ans;
	}
	// ����
	void backTrack(int x, int y,TrieNode* parent, vector<vector<char>>& b) {
		TrieNode* cur  = parent->c[b[x][y]];
		//����н������⣬Ȼ�����
		if (cur->w  != "")ans.push_back(cur->w),cur->w = "";
		//��֦����������������߽�ڵ㡣
		if (cur->c.empty()) {
			parent->c.erase(parent->c.find(b[x][y]));
			return;
		}
		//4�����������̽
		int rdif[] = { -1,1,0,0 };
		int cdif[] = { 0,0,-1,1 };
		int row, col;
		char tmp  = b[x][y];
		//·���ַ�����ر��
		b[x][y] = 'X';
		for (int i  = 0; i  < 4; i++) {
			row  = x  + rdif[i];
			col  = y  + cdif[i];
			if (row  < 0 || row  >= b.size() || col  < 0 || col  >= b[0].size())continue;
			if (cur->c.count(b[row][col])) {
				backTrack(row, col, cur,b);
			}
		}
		//��ԭƥ��·���ַ���
		b[x][y] = tmp;
	}
};