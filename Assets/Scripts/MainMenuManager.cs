using System.Collections.Generic;
using Solana.Unity.Programs;
using Solana.Unity.Rpc.Core.Http;
using Solana.Unity.Rpc.Models;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI walletAddress;
    public GameObject connectWalletButton;
    public GameObject burnDWHButton;
    public GameObject notEnoughDWH;

    public async void ConnectWallet()
    {
        await Web3.Instance.LoginWalletAdapter();
        walletAddress.text = Web3.Account.PublicKey.ToString();
        connectWalletButton.SetActive(false);

        var tokens = await Web3.Rpc.GetTokenAccountsByOwnerAsync
        (
            Web3.Account.PublicKey, 
            tokenProgramId: "TokenkegQfeZyiNwAJbNbGKPFXCWuBvf9Ss623VQ5DA"
        );

        bool DWHExists = false;
        foreach (TokenAccount account in tokens.Result?.Value)
        {
            if (account.Account.Data.Parsed.Info.Mint == "DEVwHJ57QMPPArD2CyjboMbdWvjEMjXRigYpaUNDTD7o")
            {
                DWHExists = true;
                decimal DWHBal = account.Account.Data.Parsed.Info.TokenAmount.AmountDecimal;
                Debug.Log($"DWH Balance: {DWHBal}");
                if (DWHBal >= 69) burnDWHButton.SetActive(true);
                else notEnoughDWH.SetActive(true);
            }
        }
        if(!DWHExists) notEnoughDWH.SetActive(true);
    }

    public async void BurnDWH()
    {
        PublicKey DWHATA = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount
        (Web3.Account.PublicKey, 
        new("DEVwHJ57QMPPArD2CyjboMbdWvjEMjXRigYpaUNDTD7o")
        );

        var tx = new Transaction()
        {
            FeePayer = Web3.Account.PublicKey,
            Instructions = new List<TransactionInstruction>(),
            RecentBlockHash = await Web3.BlockHash()
        };

        tx.Add(TokenProgram.BurnChecked(
            new("DEVwHJ57QMPPArD2CyjboMbdWvjEMjXRigYpaUNDTD7o"),
            DWHATA,
            Web3.Account.PublicKey,
            (ulong)(69 * Mathf.Pow(10, 6)),
            6));

        RequestResult<string> res = await Web3.Wallet.SignAndSendTransaction(tx);
        Debug.Log($"Result: {Newtonsoft.Json.JsonConvert.SerializeObject(res)}");

        if (res.WasSuccessful && res.WasRequestSuccessfullyHandled && res.WasHttpRequestSuccessful) SceneManager.LoadScene("Game");
    }
}