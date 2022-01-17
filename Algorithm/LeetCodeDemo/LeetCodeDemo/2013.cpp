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
//…Ëº∆Ã‚
class DetectSquares {
    vector<set<int> > rows;
    vector<set<int> > cols;
    set<pair<int,int> > anss;
public:
    DetectSquares() {
        rows.clear();
        cols.clear();
        anss.clear();
    }

    void add(vector<int> p) {
        rows[p[0]].insert(p[1]);
        cols[p[1]].insert(p[0]);
    }

    int count(vector<int> p) {
        for(int i=0;i<rows[p[0]].size();i++)
        {
            
        }
    }
};