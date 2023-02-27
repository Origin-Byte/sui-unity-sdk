# sui-unity-sdk

Connecting Unity game developers to Sui and Origin Byte's NFT ecosystem.

# Features
- For Rpc clients’ direct interaction with the Sui JSON-RPC https://docs.sui.io/sui-jsonrpc
    - Read API
    - Event Read API
    - Transaction Builder Api
    - Event streaming support
    - Helper methods to build and execute transactions
    - Typed move calls
- Wallet Management 
    - Generate and Restore key pairs with Mnemonics (currently Ed25519 supported)
        - [SLIP-0010](https://github.com/satoshilabs/slips/blob/master/slip-0010.md) : Universal private key derivation from master private key also supported
    - Sign transactions
    - Store key pair in PlayerPrefs
- Interact with Origin Byte Nft Protocol (0.22) https://github.com/Origin-Byte/nft-protocol
- Helper Scripts and prefabs to load NFTs (even Capys!)
- Windows desktop and WebGL platforms tested
- Unity 2021.3.10f1 LTS or later supported
- Samples are using Sui 0.27.0 devnet

![Capy Image Nft loader](/imgs/capy_loader_1.webp "Capy Image Nft loader")

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

# Keep in mind: Sui DevNet resets frequently, and the owned Coins, Nfts will be erased. Also, the package ids have to be updated with newly published versions as well.

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
    var moveCallTx = new MoveCallTransaction()
    {
        Signer = signer,
        PackageObjectId = "0x2554106d7db01830b6ecb0571c489de4a3999163",
        Module = "counter",
        Function = "increment",
        TypeArguments = ArgumentBuilder.BuildTypeArguments(),
        Arguments = ArgumentBuilder.BuildArguments( SharedCounterObjectId ),
        Gas = (await SuiHelper.GetCoinObjectIdsAboveBalancesOwnedByAddressAsync(SuiApi.Client, signer, 1, 10000))[0],
        GasBudget = 5000,
        RequestType = SuiExecuteTransactionRequestType.WaitForEffectsCert
    };
    
    await SuiApi.Signer.SignAndExecuteMoveCallAsync(moveCallTx);
```

## Mint Nft using Origin Byte Nft Protocol

![Alt text](/imgs/nft_protocol_mint.png "Claim Nft using Origin Byte Nft Protocol")

Our SDK contains the NftProtocolClient class that provide access to Sui using the Nft Protocol.

This sample demonstrates the minting of an Nft using Origin Byte Nft protocol with RPC calls.
More info on the protocol can be found here: https://github.com/Origin-Byte/nft-protocol
This sample uses a pre-minted collection of [DEADBYTES](https://github.com/tomfurrier/nft-protocol/blob/dead-bytes/sources/collection/deadbytes.move).


### Setup your collection
1. Publish https://github.com/tomfurrier/nft-protocol/blob/dead-bytes/sources/collection/deadbytes.move or a similar, std_collection. More information about the [nft protocol](https://github.com/Origin-Byte/nft-protocol) .
2. update the MintCapId and signer if needed
3. Mint Nfts by calling NftProtocolClient.MintNftAsync

```csharp
    // we can sign the mint transaction with the wallet that deployed the contract
	var signerKeyPair = Mnemonics.GetKeypairFromMnemonic(SAMPLE_SIGNER_MNEMONIC);
	var signer = new Signer(SuiApi.Client, signerKeyPair);
	var nftProtocolClient = new NftProtocolClient(SuiApi.Client, signer);

	var randomFaceIndex = Random.Range(1, 9);
	var txParams = new MintNft()
	{
		Attributes = new Dictionary<string, object>()
		{
			{ "nft_type", "face" },
		},
		Description = "You can use this as a face of your character in the game!",
		MintCap = NFTMintCapIdField.text,
		Recipient = TargetWalletAddressInputField.text,
		ModuleName =  deadBytesType.Module,
		Name = $"Face {randomFaceIndex}",
		PackageObjectId = mintCapType.PackageId,
		Signer = signerKeyPair.PublicKeyAsSuiAddress,
		Url = $"https://suiunitysdksample.blob.core.windows.net/nfts/face{randomFaceIndex}.png"
	};

	// if we pass null, it will automatically select a gas object
	var mintRpcResult = await nftProtocolClient.MintNftAsync(txParams, null);
```

## Nft loaders

Check out the NftLoaders scene. Contains samples for using Nfts as UI Image sprite, main texture of material.

![Nft loaders](/imgs/nftloaders.png "Nft loaders")

Change the Ids and Address to yours.

![UI Image Nft loader](/imgs/ui_image_nft_loader.png "UI Image Nft loader")


![UI Image Nft loader for address](/imgs/ui_image_nft_loader_for_address.png "UI Image Nft loader for address")


![UI Image Nft loader for address](/imgs/texture_nft_loader.png "Texture nft loader")

## Capy Loader

You can also import Capys! This uses the experimental Vector Graphics package to render the SVG images as PNGs. Check out the CapyNftLoaders scene to see it in action!

![Capy Image Nft loader](/imgs/capy_loader_1.webp "Capy Image Nft loader")
![Capy Image Nft loader configuration](/imgs/capy_loader_2.webp "Capy Image Nft loader configuration")

The srcipt can load the whole hierarchy of accessories for the Capy, or just the main Capy object.

# Dependencies

Every dependency used by the SDK can be found in ./Assets/SuiUnitySDK/Plugins.

Nuget packages are in ./Assets/SuiUnitySDK/Plugins/NuGetPackages.

- `suinet`, our internal C#/.NET Sui library. It will be regularly updated as features are added to the core library.
- `Chaos.NaCl.Standard`
- `Portable.BouncyCastle`
- `Newtonsoft.Json` (included in recent Unity versions by default)
- `com.unity.vectorgraphics`
    - if you got compilation errors, you need to add this entry to your .Packages/manifest.json file's dependencies section:

    `"com.unity.vectorgraphics": "2.0.0-preview.20"`

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
