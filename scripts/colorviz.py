import pickle as pkl
import numpy as np
from sklearn.decomposition import PCA
import random

with open('ent_col.pkl','rb') as f:
 x = pkl.load(f)

emb = []
cols= []

for i in random.sample(list(x.keys()),14000):
 if x[i][1]=='#CCCC00':
  print(i)
 emb.append(x[i][0])
 cols.append(x[i][1])


dic = dict()

for i in range(len(cols)):
 if cols[i] in dic:
  dic[cols[i]].append(i)
 else:
  dic.update({cols[i]:[i]})

emb = np.array(emb)

pca = PCA(n_components=3)
pca.fit(emb)
emb = pca.transform(emb)

from mpl_toolkits.mplot3d import Axes3D
import matplotlib.pyplot as plt

fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')

print(dic.keys())
for i in list(dic.keys()):
 #if not i == '#CCCC00':
 # continue
 temp = dic[i]
 x =[]
 y =[]
 z =[]
 for j in temp:
  x.append(emb[j][0])
  y.append(emb[j][1])
  z.append(emb[j][2])
 ax.scatter(x, y, z, c=i, marker='o')
 

plt.show()
