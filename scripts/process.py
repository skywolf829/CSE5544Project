import gensim
import pandas as pd
import numpy as np
import pickle as pkl

model = gensim.models.KeyedVectors.load_word2vec_format('../../GoogleNews-vectors-negative300.bin', binary=True)
#model = {}

rel = pd.read_csv('../data/relations',sep='\t',header=None)
rel = list(set(rel[0]))

ent = pd.read_csv('../data/entities',sep='\t',header=None)
ent = list(set(ent[0]))

_dict = {}

for i in ent:
 temp = i.split()
 if temp[0] in model:
  temp2 = model[temp[0]]
 else:
  temp2 = np.zeros(300)
 for j in range(1,len(temp)):
  if temp[j] in model:
   temp2 = np.add(temp2,model[temp[j]])
 temp2 =  temp2/np.linalg.norm(temp2)
 _dict.update({i:temp2})



with open('ent.pkl', 'wb') as handle:
 pkl.dump(_dict,handle)
