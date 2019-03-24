import json, pickle, sys, numpy
with open(sys.argv[1], "rb") as fpick:
    with open(sys.argv[2] + ".json", "w") as fjson:
        dict = pickle.load(fpick)
        dict2 = {}
        #for key, value in dict.items():
        #    dict2[key.replace("'", "")] = value
        for key, value in dict.items():
            dict[key] = [value[1], value[2]]
            dict2[key.replace("'", "")] = [value[1], value[2]]
        json.dump(str(dict2), fjson)