#include "myAlgo\Structs\TreeNode.cpp"
using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <numeric>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
typedef pair<int, bool> pib;
typedef pair<int, int> pii;
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<ll, int> pli;
#define MAXN (int)(1e5+1)
#define MAXM (int)(1e5+1)
#define MOD (int)(1e9+7)

//ǰ׺�� ����ģ��
class Solution {
	struct TrieNode {
		unordered_map<char, TrieNode*> c;
		string w = "";
	};
public:
	vector<int> topStudents(vector<string>& ps, vector<string>& ns, vector<string>& rs, vector<int>& si, int k) {
		TrieNode* rootp = new TrieNode();
		//1.����ǰ׺��
		for (auto& w : ps) {//������ѯ������ÿ�����ʣ�
			TrieNode* cur = rootp;
			for (char& c : w) {//ÿ���ַ�ʱ������ַ���Ӧ�ڵ�����������ýڵ㣬�������򴴽�
				if (!cur->c.count(c)) {
					cur->c[c] = new TrieNode();
				}
				cur = cur->c[c];
			}
			cur->w = w;//����ÿ�����ʵ��ַ�������洢�õ���
		}

		TrieNode* rootn = new TrieNode();
		for (auto& w : ns) {//������ѯ������ÿ�����ʣ�
			TrieNode* cur = rootn;
			for (char& c : w) {//ÿ���ַ�ʱ������ַ���Ӧ�ڵ�����������ýڵ㣬�������򴴽�
				if (!cur->c.count(c)) {
					cur->c[c] = new TrieNode();
				}
				cur = cur->c[c];
			}
			cur->w = w;//����ÿ�����ʵ��ַ�������洢�õ���
		}
		//2.���������򣬴����ڸ��ڵ��ϵ��ַ��ڵ㿪ʼ���л��ݱȶ�
		function<bool(string,TrieNode*)> dfs = [&](string word, TrieNode* r) {
			TrieNode* cur = r;
			for (int i = 0; i < word.size(); i++) {
				if (!cur->c.count(word[i]))
					return false;
				cur = cur->c[word[i]];
			}
			return cur->w != "";
			};
		int n = rs.size();
		string tmps, s;
		int head;
		vector<pii> piis;
		int cnt = 0, score;
		for (int x = 0; x < n; x++) {
			s = rs[x];
			score = 0;
			for (int i = 0; i < s.size(); ) {
				if (i == 0 || s[i - 1] == ' ') {
					head = i;
					while (i < s.size()) {
						if (s[i] == ' ') {
							break;
						}
						i++;
					}
					tmps = s.substr(head, i - head);
					if (dfs(tmps,rootp)) {
						score += 3;
					}
					if (dfs(tmps,rootn)) {
						score -= 1;
					}
				}
				else
					i++;
			}
			piis.push_back({ score,si[x] });
		}
		sort(piis.begin(), piis.end(), [](pii& a, pii& b) {return a.first > b.first || a.first == b.first && a.second < b.second; });
		vector<int> rst(k);
		for (int i = 0; i < k && i < n; i++) {
			rst[i] = piis[i].second;
		}
		return rst;
	}
};

//��ϣ ����
//class Solution {
//public:
//    vector<int> topStudents(vector<string>& ps, vector<string>& ns, vector<string>& rs, vector<int>& si, int k) {
//		unordered_set<string> stp,stn;
//		for (auto& w : ps) {
//			stp.insert(w);
//		}
//		for (auto& w : ns) {
//			stn.insert(w);
//		}
//		int n = rs.size();
//		string tmps,s;
//		int head;
//		vector<pii> piis;
//		ll score;
//		for (int x = 0; x < n;x++) {
//			s = rs[x];
//			score = 0;
//			for (int i = 0; i < s.size(); ) {
//				if (i == 0 || s[i - 1] == ' ') {
//					head = i;
//					while (i < s.size()) {
//						if (s[i] == ' ') {
//							break;
//						}
//						i++;
//					}
//					tmps = s.substr(head, i-head);
//					if (stp.count(tmps)) {
//						score += 3;
//					}
//					if (stn.count(tmps)) {
//						score -= 1;
//					}
//				}
//				else
//					i++;
//			}
//			piis.push_back({ score,si[x] });
//		}
//		sort(piis.begin(), piis.end(), [](pii& a, pii& b) {return a.first > b.first || a.first == b.first && a.second < b.second; });
//		vector<int> rst(k);
//		for (int i = 0; i < k && i < n; i++) {
//			rst[i] = piis[i].second;
//		}
//		return rst;
//	}
//};