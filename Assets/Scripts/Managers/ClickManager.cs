﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Service;
using Map;

namespace Manager
{
    public class ClickManager : Singleton<ClickManager>
    {
        public event System.Action onClick;

        public void ClickPlayerBlock(Collider[] hitColliders)
        {
            foreach (Collider hit in hitColliders)
            {
                Block targetBlock = hit.GetComponent<Block>();
                if (!BlockManager.Instance.selectBlock.transform.name.Equals(hit.transform.name) && targetBlock.blockType.Equals(BlockManager.Instance.selectBlock.blockType)
                    && targetBlock.animalLevel.Equals(BlockManager.Instance.selectBlock.animalLevel) && targetBlock.animalIndex.Equals(BlockManager.Instance.selectBlock.animalIndex))
                {
                    MixBlock(targetBlock);
                    return;
                }
            }
        }

        public void ClickNoneBlock()
        {
            if (LevelManager.Instance.resource >= LevelManager.Instance.priceAnimal)
            {
                LevelManager.Instance.IncreaseSupply();
                LevelManager.Instance.DecreaseResource(LevelManager.Instance.priceAnimal);
                BlockManager.Instance.selectBlock.blockType = Block.Type.Player;
                BlockManager.Instance.animalBlock.Add(BlockManager.Instance.selectBlock);
                CreateAnimal();
            }
            onClick?.Invoke();
        }

        void CreateAnimal()
        {
            BlockManager.Instance.selectBlock.animalIndex = Random.Range(0, AnimalInformation.Instance.level[BlockManager.Instance.selectBlock.animalLevel].animalSprite.Length);
            BlockManager.Instance.selectBlock._spriteRenderer.sprite = AnimalInformation.Instance.level[BlockManager.Instance.selectBlock.animalLevel].animalSprite[BlockManager.Instance.selectBlock.animalIndex];
        }

        void MixBlock(Block targetBlock)
        {
            LevelManager.Instance.DecreaseSupply();
            BlockManager.Instance.selectBlock.animalLevel++;
            CreateAnimal();

            targetBlock.animalLevel = 0;
            targetBlock.animalIndex = 0;
            targetBlock.blockType = Block.Type.None;
            targetBlock._spriteRenderer.sprite = AnimalInformation.Instance.noneSprite;
            BlockManager.Instance.animalBlock.Remove(targetBlock);
            onClick?.Invoke();
        }
    }
}