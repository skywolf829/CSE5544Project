import json, pickle, sys, numpy

with open(sys.argv[1], "rb") as fpick:
    with open(sys.argv[1].split(".")[0] + ".json", "w") as fjson:
        json.dump(str(pickle.load(fpick)), fjson)