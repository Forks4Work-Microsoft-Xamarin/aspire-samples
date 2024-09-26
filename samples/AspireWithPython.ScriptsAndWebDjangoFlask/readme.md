# Aspire in Heterogenous Environment

User/Community feedback

## Prerequisites

Sample assumes that user has Python installed (`python3`)

## Recommendations (Tips and Tricks)

User should install and activate python venv in each folder to isolate package depdendencies.
 
```
└── Python
    ├── django
    ├── flask
    └── simple-machine-learning
```

https://packaging.python.org/en/latest/guides/installing-using-pip-and-virtual-environments/


Note: automatic venv installation is being tested.

### Windows

```pwsh
py -m venv .venv
.venv\Scripts\activate
python3 -m pip install --upgrade pip
python3 -m pip --version
```

### MacOSX and Linux


```bash
python3 -m venv .venv
source .venv/bin/activate
python3 -m pip install --upgrade pip
python3 -m pip --version```

