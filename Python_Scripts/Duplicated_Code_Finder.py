def find_replicated_codes(input_string):
    # Split the input string by new lines to get individual codes
    input_codes = input_string.split()

    # Dictionary to count occurrences of each code
    code_count = {}

    # Count occurrences of each code
    for code in input_codes:
        if code in code_count:
            code_count[code] += 1
        else:
            code_count[code] = 1

    # Print codes that appear more than once
    print("Replications (duplicates):")
    for code, count in code_count.items():
        if count > 1:
            print(f"{code}: {count} times")

# Input string as provided
input_string = """22I12576
23E493
23E594
23E595
23E615
23E634
23E655
23E661
23E662
23E672
23E689
23E720
23E723
23E728
23E746
23E760
23E780
23E781
23E816
23I1003
23I1005
23I10051
23I1006
23I10061
23I1012
23I1018
23I1035
23I1042
23I1053
23I1054
23I10541
23I1057
23I10571
23I1058
23I1059
23I10591
23I1087
23I1094
23I1103
23I11031
23I1107
23I1109
23I1112
23I1133
23I1152
23I1154
23I1173
23I1178
23I1179
23I1182
23I1217
23I1222
23I1235
23I1239
23I1240
23I1243
23I1247
23I1250
23I1252
23I1255
23I1266
23I1287
23I1322
23I1333
23I1336
23I1339
23I1341
23I1347
23I1366
23I1369
23I1378
23I1382
23I1389
23I1394
23I1397
23I1431
23I1432
23I1439
23I1440
23I1482
23I1488
23I1489
23I1490
23I1497
23I1499
23I1508
23I1511
23I1512
23I1513
23I1516
23I1519
23I1520
23I1522
23I1523
23I1524
23I1528
23I1529
23I1556
23I1560
23I15601
23I1561
23I157
23I1571
23I1597
23I1599
23I1613
23I1617
23I1620
23I1624
23I1626
23I1628
23I16281
23I1648
23I1670
23I1673
23I1675
23I1680
23I1693
23I1710
23I1717
23I1719
23I1762
23I1764
23I1766
23I1772
23I1774
23I1775
23I1788
23I1789
23I1790
23I1791
23I1792
23I1794
23I1796
23I1803
23I1804
23I1807
23I1822
23I1839
23I1841
23I1846
23I1882
23I1883
23I1886
23I1889
23I1891
23I1892
23I1897
23I1898
23I1935
23I1939
23I1965
23I1973
23I1974
23I1975
23I1986
23I2020
23I2024
23I2026
23I2032
23I2034
23I2035
23I2037
23I2039
23I2040
23I2044
23I2047
23I2048
23I2049
23I2094
23I2095
23I2112
23I2114
23I2132
23I2134
23I2144
23I2151
23I2171
23I2474
23I2512
23I2798
23I2806
23I2822
23I2914
23I2915
23I2998
23I3021
23I3025
23I3031
23I3056
23I3058
23I3065
23I3080
23I3081
23I3084
23I3122
23I3126
23I3186
23I3210
23I3240
23I3254
23I3260
23I3263
23I3268
23I3305
23I3315
23I3321
23I3322
23I3323
23I4791
23I515
23I528
23I5281
23I585
23I611
23I615
23I664
23I665
23I6651
23I698
23I708
23I711
23I717
23I723
23I725
23I726
23I729
23I736
23I739
23I741
23I764
23I774
23I780
23I781
23I786
23I810
23I8101
23I818
23I829
23I841
23I8411
23I842
23I845
23I848
23I888
23I893
23I900
23I9001
23I952
23I980
23I988
23I992
23O411
23X562
23X600
23X700
a22I12576
a23I1006
a23I1103
a23I1178
a23I1182
a23I1235
a23I1240
a23I1243
a23I1247
a23I1252
a23I1347
a23I1369
a23I1382
a23I1431
a23I1439
a23I1482
a23I1488
a23I1497
a23I1511
a23I1512
a23I1513
a23I1519
a23I1520
a23I1523
a23I1529
a23I1597
a23I1620
a23i1648
a23I1673
a23I1710
a23I1772
a23I1789
a23I1791
a23I1794
a23I1804
a23I1807
a23I1839
a23I1886
a23I1889
a23I1897
a23I1975
a23I1983
a23I1986
a23I2035
a23I2039
a23I2047
a23I2049
a23I2054
a23I2094
a23I2822
a23I2914
a23I2998
a23I3025
a23I3056
a23I3080
a23I3081
a23I3084
a23I3126
a23I3186
a23I3254
a23I3260
a23I3263
a23I418
a23I615
a23I717
a23I726
a23I739
a23I841
a23I900
a23I980
b22I12576
b23I1006
b23I1178
b23I1235
b23I1243
b23I1252
b23I1369
b23I1382
b23I1431
b23I1439
b23I1482
b23I1488
b23I1511
b23I1512
b23I1513
b23I1520
b23I1529
b23I1597
b23I1620
b23I1772
b23I1788
b23I1789
b23I1796
b23I1804
b23I1807
b23I2049
b23I2054
b23I2914
b23I3056
b23I3081
b23I3186
b23I726
b23I739
b23I841
b23I900
c23I1006
c23I1178
c23I1235
c23I1252
c23I1369
c23I1439
c23I1482
c23I1488
c23I1512
c23I1513
c23I1520
c23I1529
c23I1620
c23I1772
c23I1788
c23I1796
c23I1804
c23I1807
c23I2054
c23I3081
c23I726
d23I1006
d23I1178
d23I1235
d23I1369
d23I1439
d23I1488
d23I1512
d23I1513
d23I1529
d23I1620
d23I1772
d23I1796
d23I1804
d23I3081
e23I1178
e23I1369
e23I1512
e23I1529
e23I1620
e23I1772
e23I1796
e23I1804
f23I1178
f23I1369
f23I1529
f23I1772
f23I1796
f23I1804
g23I1178
g23I1369
g23i1529
g23I1620
g23I1772
g23I1796
g23I1804
h23i1178
h23I1529
h23I1772
h23I1796
h23I1804
i23I1529
i23I1804
j23I1529
j23I1804
22E10027
22E10050
22E7324
22E7326
22E7333
22E7341
22E7345
22E7347
22E7363
22E7371
22E7417
22E7423
22E7438
22E7502
22E7506
22E7508
22E7509
22E7511
22E7528
22E7558
22E7561
22E7591
22E7592
22E7596
22E7634
22E7639
22E7648
22E7672
22E7678
22E7683
22E7703
22E7705
22E7731
22E7763
22E7775
22E7795
22E7845
22E7861
22E7877
22E7880
22E7881
22E7887
22E7898
22E7910
22E7912
22E7915
22E7917
22E7951
22E7952
22E7955
22E7966
22E7977
22E7978
22E7979
22E7995
22E8025
22E8049
22E8050
22E8054
22E8056
22E8062
22E8063
22E8064
22E8113
22E8117
22E8123
22E8131
22E8132
22E8144
22E8145
22E8154
22E8167
22E8184
22E8185
22E8198
22E8199
22E8208
22E8243
22E8263
22E8276
22E8279
22E8289
22E8301
22E8304
22E8305
22E8310
22E8332
22E8333
22E8342
22E8348
22E8365
22E8367
22E8368
22E8396
22E8405
22E8406
22E8417
22E8419
22E8421
22E8426
22E8440
22E8441
22E8442
22E8444
22E8456
22E8465
22E8471
22E8480
22E8498
22E8500
22E8503
22E8515
22E8553
22E8564
22E8570
22E8630
22E8637
22E8648
22E8649
22E8674
22E8677
22E8681
22E8683
22E8697
22E8704
22E8718
22E8747
22E8763
22E8773
22E8775
22E8784
22E8792
22E8793
22E8794
22E8796
22E8797
22E8798
22E8804
22E8813
22E8817
22E8822
22E8823
22E8827
22E8829
22E8846
22E8849
22E8876
22E8882
22E8897
22E8912
22E8913
22E8933
22E8934
22E8944
22E8953
22E8956
22E8963
22E8965
22E8969
22E8970
22E8979
22E8980
22E9010
22E9015
22E9018
22E9024
22E9035
22E9040
22E9056
22E9059
22E9067
22E9075
22E9076
22E9085
22E9089
22E9094
22E9123
22E9125
22E9128
22E9144
22E9145
22E9147
22E9148
22E9154
22E9158
22E9190
22E9191
22E9195
22E9196
22E9199
22E9207
22E9208
22E9212
22E9217
22E9220
22E9228
22E9231
22E9237
22E9238
22E9242
22E9244
22E9252
22E9258
22E9261
22E9264
22E9270
22E9271
22E9272
22E9276
22E9286
22E9295
22E9305
22E9316
22E9617
22E9620
22E9637
22E9642
22E9643
22E9648
22E9649
22E9706
22E9798
22E9804
22E9823
22E9826
22E9828
22E9849
22E9850
22I11307
22I11318
22I11414
22I11508
22I11508A
22I11545
22I11566
22I11577
22I11577B
22I11581
22I11581A
22I11630
22I11636
22I11636A
22I11638
22I11638A
22I11683
22I11689
22I11691
22I11692
22I11696
22I11696A
22I11699
22I11737
22I11740
22I11740A
22I11741
22I11744
22I11746
22I11747
22I11751
22I11752
22I11774
22I11786
22I11817
22I11821
22I11822
22I11824
22I11829
22I11872
22I11878
22I11882
22I11884
22I11901
22I11904
22I11905
22I11948
22I11955
22I11960
22I11967
22I12003
22I12005
22I12024
22I12044
22I12047
22I12048
22I12134
22I12135
22I12137
22I12139
22I12140
22I12155
22I12160
22I12201
22I12202
22I12204
22I12251
22I12252
22I12257
22I12283
22I12286
22I12288
22I12306
22I12329
22I12349
22I12395
22I12396
22I12412
22I12415
22I12433
22I12446
22I12447
22I12452
22I12458
22I12511
22I12512
22I12513
22I12574
22I12575
22I125761
22I12585
22I12605
22I12606
22I12607
22I12609
22I12611
22I2274
22I9807
22I9807A
22O4414
22O4496
22O4614
22X9163
22X9168
22X9181
22X9511
22X9516
22X9517
22X9529
22X9531
22X9539
22X9573
23E3219
23E3403
23E3440
23E3463
23E3476
23E3484
23E3493
23E3505
23E3539
23E3540
23E3541
23E3551
23E3581
23E3584
23E3595
23E3604
23E3642
23E3646
23E3657
23E3659
23E3663
23E3665
23E3681
23E3683
23E3695
23E3709
23E3712
23E3714
23E3718
23E3729
23E3736
23E3759
23E3763
23E3766
23E3767
23E3781
23E3785
23E3803
23E3809
23E3815
23E3826
23E3835
23E3847
23E3857
23E3859
23E3891
23E3896
23E3917
23E3928
24I7812
24I8171
24I8143
24I6678
24I8420
24I8416
24I8070
24I6868
24I8182
24I6677
24I7804
24I7157
24I6049
24I8139
24I7813
24E4800
24I8170
24E4777
24I8287
24I8126
24I8445
24O5983
24E5022
24E4898
24E4756
24X5302
24E4753
24E4760
24I7895
24E4770
24E4769
24X5322
23I14750
23E10085
23E96091
23I14431
23I14751
23I14803
23I14798
23I14561
23I14698
23I14747
23I14754
23I14752
23I14631
23I14568
23I10832
23I10662
23I10819
23I10708   
23I9130
23I10764
23I10820
23I10710
23I10813
23E7659
23I10804
23I10683
23I10514
23I10473
23I10500
23I10505
23I10208
23I10513
23I10233
23I10283
23I10509
23I10454
23I10538
23I10527
23I10407
23I10265
23I105052
23I104542
23I10587
23I10592
23I10599
23I10511
23I10639
23I91302
23I10652
23I105992
23I106092
23I106522
23I10284
23I10545
23I102082
23I105112
23I10198
23I10294
23I10276
23I10285
23I10338
23I010101
23I10339
23I102762
23I102252
23I9762
23I9886
23I10242
23I10230
23I10201
23I97622
23I10221
23I10154
23I102083
23I102253
23I10205
23I10220
23I10142
23I10206
23I91303
23I10204
23I10146
23I10207
23I102084
23I102062
23I9913
23I102012
23I101982
23I10033
23I101462
23I10160
23I10158
23I101542
23I9354
23I101463
23I101422
23I10141
23I10092
23I10111
23I9129
23I10093
23I10090
23I9951
23I97623
23I9915
23I97624
23I10063
23I10031
23I10045
23I10037
23I10040
23I100312
23I10035
23I100313
23I10003
23I9973
23I9964
23I9974
23I9919
23I9882
23I9823
23I13270
23I12961
23I13300
23I13331
23I12582
23I13364
23I13389
23E8392
23E8397
23E8415
23E8439
23E84712
23E8504
23E8512
23E84711
23E8520
23E8528
23E8557
23E8577
23E8549
23E8183
23E8204
23E8273
23E8281
23E8295
23E8326
23E8349
23E8353
23E8374
23E8375
23E7862
23E7864
23E7869
23E7971
23E7985
23E7995
23E8059
23E8076
23E8102
23E8131
23E7732
23E7747
23E7834
23E7849
23E9123
23E9127
23E9130
23E9164
23E9161
23E9202
23E9213
23E9239
23E9244
23E9246
23E9255
23E9268
23E9267
23E8836
23E8839
23E8871
23E8878
23E8841
23E8927
23E8969
23E9051
23E9054
23E9080
23E8617
23E8626
23E8661
23E8681
23E8695
23E8713
23E8735
23E8739
23E8745
23E8771
23E8772
23E8786
23E8797
23E8802
23E8821
23E8825
23E8829
23E10006
23E10053
23E10054
23E9862
23E9609
23E10073
23E10079
23E10100
23E10129
23E98621
23E9923
23E9961
23E9998
23E9678
23E9691
23E9710
23E9713
23E9733
23E9770
23E9780
23E97801
23E97802
23E9829
23E9492
23E9530
23E9565
23E9579
23E9585
23E9613
23E9617
23E960911
23E96092
23E96093
23E9674
23E9293
23E9294
23E9295
23E9306
23E9349
23E9355
23E9366
23E9376
23E9398
23E9421
23E9447
23E9448
23E5482
23E54682
23E5518
23E5528
23E54681
23E5567
23E5599
23E5647
23E5732
23E5754
23E5149
23E5195
23E5201
23E5246
23E5264
23E5282
23E5284
23E5296
23E4880
23E4928
23E4943
23E4954
23E4979
23E5029
23E5049
23E5073
23E5070
23X9800
23I14335
23I132702
23I13611
23I13578
23I14378
23I14215
23I14397
23I14436
23I13889
23I142151
23I14467
23I144316
23I14486
23I14492
23I14493
23I14488
23I144921
23I144931
23I143971
23I144311
23I14269
23I13269
23I14514
23I144312
23I144313
23I144881
23I144314
23I144932
23I14070
23I14208
23I14209
23I140701
23I13887
23I132701
23I138871
23I14182
23I136111
23I142152
23I14225
23I13547
23I135781
23I142153
23I14272
23I14273
23I14217
23I14220
23I13965
23I14302
23I13567
23I14065
23I14071
23I14013
23I14073
23I14124
23I14128
23I140703
23I13796
23I135671
23I14019
23I138891
23I141281
23I141241
23I14125
23I13894
23I13899
23I13888
23I137961
23I13815
23I13633
23I13864
23I13629
23I13792
23I13570
23I13486
23I13618
23I13665
23I13696
23I13689
23I13739
23I13435
23I13383
23I13437
23I13438
23I13257
23I13442
23I13470
23I13382
23I134861
23I13488
23I132691
23I13544
23I13531

"""

# Find and print replicated codes
find_replicated_codes(input_string)