import pandas as pd
import pickle
import numpy as np
from sklearn.decomposition import PCA
import random
from matplotlib import pyplot as plt


x =pd.read_csv('../data/total.csv',header=None,sep='\t')

with open('../embeddings/kg_ent.pkl','rb') as f:
	dic = pickle.load(f)


#change predicate here
predicate = 'award'

x = x[x[2]=='award']

sub = list(x[0])

ob = list(x[1])

emb1 = []
emb2 = []

for i in range(len(x)):
 emb1.append(dic[sub[i]])
 emb2.append(dic[ob[i]])

emb1+=emb2

pca = PCA(n_components=2)
pca.fit(np.array(emb1))
emb = pca.transform(np.array(emb1))


emb1 = emb[:int(len(emb)/2),:]
emb2 = emb[int(len(emb)/2):,:]


x1 = []
x2 = []
y1 = []
y2 = []
z1 = []
z2 = []

for i in range(int(len(emb1)/10)):
 x1.append(emb1[i][0])
 x2.append(emb2[i][0])
 y1.append(emb1[i][1])
 y2.append(emb2[i][1])
 z1.append(emb1[i][1])
 z2.append(emb2[i][1])
'''
plt.scatter(x1,y1,c = 'red')
plt.scatter(x2,y2,c = 'blue')
#for i in range(int(len(emb1)/10)):
# plt.plot([x1[i],x2[i]],[y1[i],y2[i]])
plt.show()
'''
from mpl_toolkits.mplot3d import Axes3D
import matplotlib.pyplot as plt

fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')
ax.scatter(x1,y1,z1,c = 'red')
ax.scatter(x2,y2,z2,c = 'blue')

'''
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
 '''
plt.show()
'''
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
plt.scatter(x, y, c=i, marker='o')
'''
	
