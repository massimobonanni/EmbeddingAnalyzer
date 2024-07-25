# Embedding Analyzer Console

This console application provides various commands to analyze embedding files.

## Commands

### `calculate-distance`

The `calculate-distance` command is used to calculate the cosine distance between two texts given in input.

#### Syntax:
Parameters:
- `--endpoint -e`: The endpoint of Azure OpenAI resource.
- `--api-key -k`: The API key of Azure OpenAI resource.
- `--model-name -m`: The deploy name of the embeddeding model in Azure OpenAI resource.
- `--text1 -t1`: the first text.
- `--text2 -t2`: the second text.

Example:
``` bash
.\EmbeddingAnalyzer.exe calculate-distance -e https://myresource.openai.azure.com/ -k 643c3de9d8......991be924be9 -m myEmbedding -t1 "mother" -t2 "dad"
```

### `rag`

The `rag` command, given one or more texts and a text file with several texts, return the top sentences order by distance for each text in input.

#### Syntax:
Parameters:
- `--endpoint -e`: The endpoint of Azure OpenAI resource.
- `--api-key -k`: The API key of Azure OpenAI resource.
- `--model-name -m`: The deploy name of the embeddeding model in Azure OpenAI resource.
- `--text -t`: The text used to seach in the data file. You can add multiple `--text` parameters.
- `--file -f`: The full path of the line feed file (.txt) contains all the text to use as knowledge base.
- `--top -t`: The number of result to retrieve. Default is 5.

Example:
``` bash
.\EmbeddingAnalyzer.exe rag -e https://myresource.openai.azure.com/ -k 643c3de9d8......991be924be9 -m myEmbedding -t "bycicle" -t "uncle" -f "C:\data.txt" -t 5
```

If the `data.txt` file is the following ([link](Samples/Other/CommonWords.txt)):

```
Dog
Cat
Fish
Bird
Car
Airplane
Bus
Train
Mother
Father
Sister
Brother
Grandmother
```

The result will look like the following:

```	
Get Embedding from input texts...
	Embedding 'Bicycle'... Cost 2 tokens
	Embedding 'Uncle'... Cost 2 tokens

Get Embedding from input file...
	Embedding 'Dog'... Cost 1 tokens
	Embedding 'Cat'... Cost 1 tokens
	Embedding 'Fish'... Cost 1 tokens
	Embedding 'Bird'... Cost 1 tokens
	Embedding 'Car'... Cost 1 tokens
	Embedding 'Airplane'... Cost 2 tokens
	Embedding 'Bus'... Cost 1 tokens
	Embedding 'Train'... Cost 1 tokens
	Embedding 'Mother'... Cost 1 tokens
	Embedding 'Father'... Cost 1 tokens
	Embedding 'Sister'... Cost 2 tokens
	Embedding 'Brother'... Cost 2 tokens
	Embedding 'Grandmother'... Cost 2 tokens

Calculate distances...

Calculating distances for 'Bicycle'...
	Calculating distances from 'Dog'... distance 0,23434514
	Calculating distances from 'Cat'... distance 0,21297699
	Calculating distances from 'Fish'... distance 0,20633471
	Calculating distances from 'Bird'... distance 0,189928
	Calculating distances from 'Car'... distance 0,1882863
	Calculating distances from 'Airplane'... distance 0,14195889
	Calculating distances from 'Bus'... distance 0,18443912
	Calculating distances from 'Train'... distance 0,17284805
	Calculating distances from 'Mother'... distance 0,2300654
	Calculating distances from 'Father'... distance 0,21888173
	Calculating distances from 'Sister'... distance 0,19546777
	Calculating distances from 'Brother'... distance 0,1933977
	Calculating distances from 'Grandmother'... distance 0,22457987

Calculating distances for 'Uncle'...
	Calculating distances from 'Dog'... distance 0,2113707
	Calculating distances from 'Cat'... distance 0,20734459
	Calculating distances from 'Fish'... distance 0,22081673
	Calculating distances from 'Bird'... distance 0,20716733
	Calculating distances from 'Car'... distance 0,21843219
	Calculating distances from 'Airplane'... distance 0,20739484
	Calculating distances from 'Bus'... distance 0,22874707
	Calculating distances from 'Train'... distance 0,20471978
	Calculating distances from 'Mother'... distance 0,1850723
	Calculating distances from 'Father'... distance 0,14244223
	Calculating distances from 'Sister'... distance 0,13531882
	Calculating distances from 'Brother'... distance 0,115189016
	Calculating distances from 'Grandmother'... distance 0,1304841

Listing top 5 for 'Bicycle'...
	Distance from 'Airplane' - 0,14195889
	Distance from 'Train' - 0,17284805
	Distance from 'Bus' - 0,18443912
	Distance from 'Car' - 0,1882863
	Distance from 'Bird' - 0,189928

Listing top 5 for 'Uncle'...
	Distance from 'Brother' - 0,115189016
	Distance from 'Grandmother' - 0,1304841
	Distance from 'Sister' - 0,13531882
	Distance from 'Father' - 0,14244223
	Distance from 'Mother' - 0,1850723
```