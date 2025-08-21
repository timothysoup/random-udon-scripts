# Random Udon Scripts

This is a collection of random Udon scripts that I've made for VRChat. These scripts are free to use and modify, but please credit me if you use them in your own projects (unless I specify otherwise in the file or it is AI generated).

I am not responsible for any issues that may arise from using these scripts. Use at your own risk.

## Scripts

### `./crypto`

- AES-256 Decryptor (AI Generated): Decrypts a string using an AES-256 key and IV.

### `./teleport`

- Teleport Manager: Allows teleporting other players to specified positions and orientations in the VRChat world (and therefore to other players too).
> This was designed before Udon Networking supported events with variables. It has no queue and will not handle concurrency properly if multiple teleport requests are made at the same time. Redesign with NetworkCallable events is not planned but may occur.
