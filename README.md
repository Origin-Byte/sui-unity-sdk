# sui-unity-sdk

Connecting Unity game developers to Sui and Origin Byte's NFT ecosystem.

# Features
- For Rpc clients’ direct interaction with the Sui JSON-RPC https://docs.sui.io/sui-jsonrpc
    - Read API
    - Event Read API
    - Transaction Builder Api
- Wallet Management 
    - Generate and Restore key pairs with Mnemonics (currently Ed25519 supported)
    - Sign transactions
    - Store key pair in PlayerPrefs
- Interact with Origin Byte Nft Protocol https://github.com/Origin-Byte/nft-protocol

- Windows desktop and WebGL platforms tested
- Unity 2021.3.10f1 LTS or later supported
- Samples are using Sui 0.10.0 devnet

### Try the live example here: https://suiunitysdksample.z13.web.core.windows.net/ 

# Getting Started

## Quickstart: download this repository and open with Unity

The Samples are in ./Assets/SuiUnitySDK/Samples
Open a Scene from ./Assets/SuiUnitySDK/Samples/Scenes

## Installation
Choose one of the following methods:

### 1. The package is available on the [openupm registry](https://openupm.com/packages/com.originbyte.suiunitysdk). You can install it via openupm-cli.
```
openupm add com.originbyte.suiunitysdk
```

### 2. Via 'Add package from git URL' in Unity Package Manager
```
https://github.com/Origin-Byte/sui-unity-sdk.git#upm
```

### 3. You can also install via git url by adding this entry in your **manifest.json**
```
"com.originbyte.suiunitysdk": "https://github.com/Origin-Byte/sui-unity-sdk.git#upm"
```

### 4. Download the latest `sui-unity-sdk.unitypackage` from the releases: https://github.com/Origin-Byte/sui-unity-sdk/releases
1. Drag and drop the `sui-unity-sdk.unitypackage` to the Project window
2. As soon as the Import Windows pop up, just click the Import button


## Start using the SDK via SuiApi and SuiWallet classes. Also check out the samples below.

# Usage Samples

All samples can be found in SuiUnitySDK/Samples

## Wallet management
![Alt text](/imgs/wallet_ui_1.png "Wallet UI")


![Alt text](/imgs/create_new_wallet.png "Create New Wallet")


![Alt text](/imgs/import_wallet.png "Import Wallet")


You can click on Create New Wallet or Import Wallet buttons to initialize the currently active Wallet.


PlayerPrefs is used as a keystore at the moment, and it saves the last active wallet and will be loaded from there on the next restart.


Now you are ready to execute transactions that require signature.

```csharp
 var mnemonics = SuiWallet.CreateNewWallet();
 ...
 SuiWallet.RestoreWalletFromMnemonics(mnemonics);
 var activeAddress = SuiWallet.GetActiveAddress();
```

## RPC Read API
![Alt text](/imgs/read_api.png "Read API test")

Enter any address and see the results as formatted JSON.

```csharp
    var ownedObjectsResult = await SuiApi.Client.GetObjectsOwnedByAddressAsync(address);
    Ouput.text = JsonConvert.SerializeObject(ownedObjectsResult.Result, Formatting.Indented);
```

## RPC Move call and execute transaction samples

![Alt text](/imgs/transactions.png "Transactions")

This sample code calls a function in a published move package that has a ```counter``` module with an ```increment``` function. It modifies a shared object, incrementing the counter by 1. 

See move logic here: https://github.com/MystenLabs/sui/blob/main/sui_programmability/examples/basics/sources/counter.move

```csharp
    var signer = SuiWallet.GetActiveAddress();
    var packageObjectId = "0xa21da7987c2b75870ddb4d638600f9af950b64c6";
    var module = "counter";
    var function = "increment";
    var typeArgs = System.Array.Empty<string>();
    var args = new object[] { SharedCounterObjectId };
    var gasObjectId = GasObjectIdInput.text;
    var rpcResult = await SuiApi.Client.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasObjectId, 2000);
    var keyPair = SuiWallet.GetActiveKeyPair();

    var txBytes = rpcResult.Result.TxBytes;
    var signature = keyPair.Sign(rpcResult.Result.TxBytes);
    var pkBase64 = keyPair.PublicKeyBase64;

    await SuiApi.Client.ExecuteTransactionAsync(txBytes, SuiSignatureScheme.ED25519, signature, pkBase64);
    ...
```

## Mint Nft using Origin Byte Nft Protocol

![Alt text](/imgs/nft_protocol_mint.png "Mint Nft using Origin Byte Nft Protocol")

This sample demonstrates the minting of an Nft for a collection using Origin Byte Nft protocol with RPC calls.
More info on the protocol can be found here: https://github.com/Origin-Byte/nft-protocol
In this sample we automatically query for 2 separate SUI coin type objects, because the move call executes in a 
batch transaction and sui does not allow the same coin object to be used as gas and mutate in the move call as well.

```csharp
    var signer = SuiWallet.GetActiveAddress();
    // package id of the Nft Protocol
    var packageObjectId = "0x1e5a734576e8d8c885cd4cf75665c05d9944ae34";
    var module = "std_nft";
    var function = "mint_and_transfer";
    var typeArgs = System.Array.Empty<string>();

    // We need 2 separate gas objects because both of them will be mutated in a batch transaction
    var gasObjectIds = await SuiHelper.GetCoinObjectIdsAboveBalancesOwnedByAddressAsync(signer, 2);

    if (gasObjectIds.Count < 2)
    {
        Debug.LogError("Could not retrieve 2 sui coin objects with at least 2000 balance. Please send more SUI to your address");
        return;
    }

    var args = new object[] { NFTNameInputField.text, NFTUrlInputField.text, false, new object[] { "description" },
        new object[] { NFTDescriptionInputField.text }, NFTCollectionIdInputField.text, gasObjectIds[0], signer };

    NFTMintedText.gameObject.SetActive(false);
    NFTMintedReadonlyInputField.gameObject.SetActive(false);
    var rpcResult = await SuiApi.Client.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasObjectIds[1], 2000);

    if (rpcResult.IsSuccess)
    {
        var keyPair = SuiWallet.GetActiveKeyPair();

        var txBytes = rpcResult.Result.TxBytes;
        var signature = keyPair.Sign(rpcResult.Result.TxBytes);
        var pkBase64 = keyPair.PublicKeyBase64;

        var txRpcResult = await SuiApi.Client.ExecuteTransactionAsync(txBytes, SuiSignatureScheme.ED25519, signature, pkBase64);
        
    }
    else
    {
        Debug.LogError("Something went wrong with the move call: " + rpcResult.ErrorMessage);
    }

```

# Dependencies

Every dependency used by the SDK can be found in ./Assets/SuiUnitySDK/Plugins.

Nuget packages are in ./Assets/SuiUnitySDK/Plugins/NuGetPackages.

- `suinet`, our internal C#/.NET Sui library. It will be regularly updated as features are added to the core library.
- `Chaos.NaCl.Standard`
- `Portable.BouncyCastle`
- `Newtonsoft.Json` (included in recent Unity versions by default)

## Samples dependencies

Dependencies used by the Samples can be found in ./Assets/SuiUnitySDK/Samples/Plugins

- `TextMesh Pro`
- `WebGLCopyAndPaste`

# Roadmap
- Mobile platform support (iOS, Android)
- WalletConnect
- Streaming RPC client, Event subscription
- Secp256k1 keypair support
- More samples
- Origin-Byte NFT ecosystem access from Unity
- Higher level APIs, easy-to-use Prefabs 
- Rust & Typescript SDK parity
