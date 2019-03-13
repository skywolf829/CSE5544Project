import json, pickle, sys, numpy

with open(sys.argv[1], "rb") as fpick:
    with open(sys.argv[1].split(".")[0] + ".json", "w") as fjson:
        dict = pickle.load(fpick)
        json.dump(str(dict), fjson)