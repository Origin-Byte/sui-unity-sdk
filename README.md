# sui-unity-sdk

Connecting Unity game developers to Sui and Origin Byte's NFT ecosystem.

## Features
- Rpc client for direct interaction with the Sui JSON-RPC API https://docs.sui.io/sui-jsonrpc
    - currently exposed: Read API, Event Read API, Transaction Builder Api
- Generate and Restore key pairs with Mnemonics (currently Ed25519 supported), signing transactions
- Currently tested on Windows desktop and WebGL platforms
- Tested with Unity 2021.3.9f1 LTS and greater

### Try the live samples here: https://suiunitysdksample.z13.web.core.windows.net/ 

## Getting Started

### To try everything: Download this repository and open with Unity

The Samples are in ./Assets/SuiUnitySDK/Samples
Open a Scene from ./Assets/SuiUnitySDK/Samples/Scenes

### Import the SDK unitypackage to your Unity project
0. Download the unity package from the releases: https://github.com/Origin-Byte/sui-unity-sdk/releases/tag/v0.0.1-alpha
    - or import via the package manager from git url: https://github.com/Origin-Byte/sui-unity-sdk.git?path=/Assets/SuiUnitySDK
1. Create an Unity project
2. Drag and drop the unitypackage to the Project window
3. As soon as the Import Windows pop up, just click the Import button
4. If prompted, import Text Mesh Pro Essentials as well
5. The SDK will be imported

### Export a unitypackage from the SDK in this repository
1. Click the RMB on the SuiUnitySDK folder to get a context menu
2. Click Export Package on the context menu
3. Uncheck the *Include dependencies* checkbox not to export non-sdk files
4. Click the Export button
5. Write a name of the package
6. Click the Save button
7. The unitypackage is saved on your disk

## Usage Samples

All samples can be found in SuiUnitySDK/Samples

### Create New Wallet and Import Wallet

![Alt text](/imgs/create_new_wallet.png "Create New Wallet")

You can click on Create New Wallet or Import Wallet buttons to initialize the currently active Wallet.
PlayerPrefs is used as a keystore at the moment, and it saves the last active wallet and will be loaded from there on the next restart.
Now you are ready to execute transactions that require signature.

```csharp
 var mnemo = Mnemonics.GenerateMnemonic();
 ...
 Mnemonics.GetKeypairFromMnemonic(mnemo).PublicKeySuiAddress;
```

### RPC Read API
![Alt text](/imgs/read_api.png "Read API test")

Enter any address and see the results as formatted JSON.

```csharp
    var rpcClient = new UnityWebRequestRpcClient(SuiConstants.DEVNET_ENDPOINT);
    var suiJsonRpcApi = new SuiJsonRpcApiClient(rpcClient);
    var ownedObjectsResult = await suiJsonRpcApi.GetObjectsOwnedByAddressAsync(Input.text);
    Ouput.text = JsonConvert.SerializeObject(ownedObjectsResult.Result, Formatting.Indented);
```

### RPC Move call and execute transaction samples

![Alt text](/imgs/transactions.png "Transactions")

This sample code calls a function in a published move package that has a ```counter``` module with an ```increment``` function. It modifies a shared object, incrementing the counter by 1. 

See move logic here: https://github.com/MystenLabs/sui/blob/main/sui_programmability/examples/basics/sources/counter.move

```csharp
    var rpcClient = new UnityWebRequestRpcClient(SuiConstants.DEVNET_ENDPOINT);
    var suiJsonRpcApi = new SuiJsonRpcApiClient(rpcClient);

    var signer = SuiWallet.Instance.GetActiveAddress();
    var packageObjectId = "0xa21da7987c2b75870ddb4d638600f9af950b64c6";
    var module = "counter";
    var function = "increment";
    var typeArgs = System.Array.Empty<string>();
    var args = new object[] { SharedCounterObjectId };
    var gasObjectId = GasObjectIdInput.text;
    var rpcResult = await suiJsonRpcApi.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasObjectId, 2000);
    var keyPair = SuiWallet.Instance.GetActiveKeyPair();

    var txBytes = rpcResult.Result.TxBytes;
    var signature = keyPair.Sign(rpcResult.Result.TxBytes);
    var pkBase64 = keyPair.PublicKeyBase64;

    await suiJsonRpcApi.ExecuteTransactionAsync(txBytes, SuiSignatureScheme.ED25519, signature, pkBase64);
    ...
```

### Mint NFT Samples

```csharp
    var rpcClient = new UnityWebRequestRpcClient(SuiConstants.DEVNET_ENDPOINT);
    var suiJsonRpcApi = new SuiJsonRpcApiClient(rpcClient);

    var signer = SuiWallet.Instance.GetActiveAddress();
    var packageObjectId = "0x2";
    var module = "devnet_nft";
    var function = "mint";
    var typeArgs = System.Array.Empty<string>();
    var nftName = "nft name";
    var nftDescription = "nft description";
    var nftUrl = "https://avatars.githubusercontent.com/u/112119979";
    var args = new object[] { nftName, nftDescription, nftUrl };
    var gasObjectId = GasObjectIdInputField.text;

    var rpcResult = await suiJsonRpcApi.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasObjectId, 2000);

    if (rpcResult.IsSuccess)
    {
        var keyPair = SuiWallet.Instance.GetActiveKeyPair();

        var txBytes = rpcResult.Result.TxBytes;
        var signature = keyPair.Sign(rpcResult.Result.TxBytes);
        var pkBase64 = keyPair.PublicKeyBase64;

        var txRpcResult = await suiJsonRpcApi.ExecuteTransactionAsync(txBytes, SuiSignatureScheme.ED25519, signature, pkBase64);
    }
    else
    {
        Debug.LogError("Something went wrong with the move call: " + rpcResult.ErrorMessage);
    }

```

## Dependencies

Every dependency used by the SDK can be found in ./Assets/SuiUnitySDK/Plugins.

Nuget packages are in ./Assets/SuiUnitySDK/Plugins/NuGetPackages

- `suinet`, our internal C#/.NET Sui library. It will be regularly updated as features are added to the core library.
- `Chaos.NaCl.Standard`
- `Portable.BouncyCastle`
- `Newtonsoft.Json` (included in recent Unity versions by default)

### Samples dependencies

Dependencies used by the Samples can be found in ./Assets/SuiUnitySDK/Samples/Plugins

- `TextMesh Pro`
- `WebGLCopyAndPaste`

## Roadmap
- Mobile platform support (iOS, Android)
- WalletConnect
- Streaming RPC client, Event subscription
- Secp256k1 keypair support
- More samples
- Origin-Byte NFT ecosystem access from Unity
- Higher level APIs, easy-to-use Prefabs 
- Rust & Typescript SDK parity
