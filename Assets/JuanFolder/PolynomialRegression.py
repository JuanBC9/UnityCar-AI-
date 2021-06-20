import numpy as np
import pandas as pd
from sklearn import linear_model

class PolynomialRegression:

    def __init__(self,degree, Data):
        self.degree = degree
        self.regr = linear_model.LinearRegression()
        self.dataset = pd.read_csv(Data, sep=" ")

        self.y = self.dataset.iloc[:,-1:].values
        XValues = self.dataset.iloc[:,0:-1].values
        
        newX = XValues.copy()

        for i in range(2, self.degree + 1):
            newX = np.concatenate((newX, XValues**i), axis = 1)

        self.X = newX


    def train_Model(self):
        self.regr.fit(self.X, self.y)
    

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

for i in range(1, 15):
	model = PolynomialRegression(i, data)
	model.train_Model()
	score = model.score()

	if(score > maxScore):
		maxScore = score
		bestDegree = i

print(bestDegree)
print(maxScore)

model = PolynomialRegression(bestDegree, data)
model.train_Model()

out = ""
out = out + str(model.regr.intercept_[0])
aux = 0

for d in range(1, bestDegree + 1):
    for i in range(0, 15):
        out = out + " + " + str(model.regr.coef_[0][aux]) + " * Math.Pow(sensorData[" + str(i) + "], " + str(d) + ")"
        aux += 1

f = open("model.txt", "w")
f.write(out)
f.close
