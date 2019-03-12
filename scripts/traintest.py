from sklearn.model_selection import train_test_split
import pandas as pd

X = pd.read_csv('total_mapped.csv',sep='\t',header=None)


X_train, X_test = train_test_split(X, test_size=0.3, random_state=42)

X_test, X_valid = train_test_split(X_test, test_size=0.5, random_state=42)

X_train.to_csv('train.txt',sep='\t',header=None,index=False)

X_test.to_csv('test.txt',sep='\t',header=None,index=False)

X_valid.to_csv('valid.txt',sep='\t',header=None,index=False)
