import numpy as np
import pandas as pd
from sklearn import linear_model

class PolynomialRegression:

    def __init__(self,degree, Data):
        self.degree = degree
        self.regr = linear_model.LinearRegression()
        self.dataset = pd.read_csv(Data, sep=" ")

        self.y = dataset.iloc[:,-1:].values
        XValues = dataset.iloc[:,0:-1].values
        
        newX = np.zeros(XValues.shape)

        for i in range(2, self.degree + 1):
            newX = np.concatenate((newX, XValues**i), axis = 1)

        self.X = newX


    def train_Model(self):
        self.regr.fit(self.X, y)
    

    def predict(self, x):

        newX = np.zeros(x.shape)

        for i in range(2, self.degree + 1):
            newX = np.concatenate((newX, x**i), axis = 1)
        
        return self.regr.predict(newX)

    def score(self):
        return self.regr.score(self.X, self.y)


data = 'Data.txt'
maxScore = -1
bestDegree = 0

for i in range(0, 15):
	model = PolynomialRegression(i, data)
	model.train_Model()
	score = model.score()

	if(score > maxScore):
		maxScore = score
		bestDegree = i

model = PolynomialRegression(bestDegree, data)
model.train_Model()

out = ""
out += model.regr.intercept_
out



f = open("model.cs", "a")