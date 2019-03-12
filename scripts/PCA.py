import numpy, pickle, sklearn, sys, os
import numpy as np
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D
from sklearn import decomposition
from sklearn import datasets
from sklearn.preprocessing import StandardScaler

loaded = False
embeddings = {}
entries = []
PCA_embeddings = {}
if os.path.getsize(sys.argv[1]) > 0:
    with open(sys.argv[1], "rb") as f:
        unpickler = pickle.Unpickler(f)
        embeddings = unpickler.load()
        loaded = True
 

if loaded:    
    pca = decomposition.PCA(n_components=int(sys.argv[2]))
    
    for key, value in embeddings.items():
        if ~numpy.isnan(value).any():
            entries.append(value)
        else:
            entries.append(np.zeros(300))
        
    entries = np.array(entries)
    embeddings_std = StandardScaler().fit_transform(entries)
    embeddings_std = pca.fit_transform(embeddings_std)
    
    i = 0
    for key, value in embeddings.items():
        PCA_embeddings[key] = embeddings_std[i]
        #print(key + " " + str(PCA_embeddings[key]))
        i = i + 1
    
    pickle.dump(embeddings_std, open(sys.argv[1].split(".")[0] + "reduced" + sys.argv[2] + ".pkl", "wb"))