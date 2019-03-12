import pandas as pd
import numpy as np
import pickle as pkl

x = pd.read_csv('total.csv',sep='\t')

ent = list(set(x['0'].append(x['1'])))
rel = list(set(x['2']))

ent_dict = {}
rel_dict = {}

for i in range(len(ent)):
 ent_dict.update({ent[i]:i})

for i in range(len(rel)):
 rel_dict.update({rel[i]:i})

with open('rel_dict.pkl','wb') as f:
 pkl.dump(rel_dict,f)

with open('ent_dict.pkl','wb') as f:
 pkl.dump(ent_dict,f)

temp1 = []
temp2 = []
temp3 = []
for i in range(len(x)):
 temp1.append(ent_dict[x.iloc[i]['0']])
 temp2.append(ent_dict[x.iloc[i]['1']])
 temp3.append(rel_dict[x.iloc[i]['2']])

x['0'] = temp1
x['1'] = temp3
x['2'] = temp2

x.to_csv('total_mapped.csv',index=False,sep='\t')
