# 環境構築

- Unity 側
  - [GitHub のUnity-Technologies / ml-agents](https://github.com/Unity-Technologies/ml-agents/blob/main/com.unity.ml-agents.extensions/Documentation~/com.unity.ml-agents.extensions.md)
- python 側
  - 上記リポジトリを展開し、カレントディレクトリを移す
  - pip install -e ./ml-agents-envs
  - pip install -e ./ml-agents
  - https://pytorch.org/get-started/locally/ のインストール手順に従ってpytorch をインストールする





mlagents-learn ./config/sac/tetris.yaml --run-id=tetris_sac --resume

