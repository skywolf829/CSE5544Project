import pandas as pd
import numpy as np
import pickle as pkl
from scipy.spatial.distance import cosine,euclidean
import math

with open('../embeddings/ent.pkl','rb') as f:
 x = pkl.load(f)

with open('../data/words.pkl','rb') as f:
 dic = pkl.load(f)

words = list(dic.keys())

colors = {0:'#CC0000',1:'#CC6600',2:'#CCCC00',3:'#66CC00',4:'#00CCCC',5:'#0066CC',6:'#CC00CC',7:'#CC0066',8:'#606060',9:'#666600',10:'#FFE5CC',11:'#990000'}

# red, orange, lgreen, dgreen, lblue, dblue, magenta, pink, grey 

n_dic = dict()

for i in list(x.keys()):
 if math.isnan(x[i][0]):
  continue
 idx = -1
 temp = 100000
 for j in range(len(words)):
  if cosine(dic[words[j]],x[i]) < temp:
   temp = euclidean(dic[words[j]],x[i])
   idx = j
 n_dic.update({i:[x[i],colors[idx],idx]})

with open('ent_col_eu.pkl','wb') as f:
 pkl.dump(n_dic,f)
