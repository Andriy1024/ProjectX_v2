```
npm init
npm i -g typescript
tsc --init --sourceMaps --rootDir src --outDir dist
tsc

npm i --save-dev typescript
npm i -D ts-node
```

launch.json:
```
{
    "configurations": [
        {
            // To support debigging with multiple files
            "program": "${workspaceFolder}\\dist\\index.js"
        }
    ]
}
Run: F5
```

ctrl+shift+p or F1:
```
Tasks: Configure Default Build Task
tsc: watch - tsconfig.json
Tasks: Run Build Task
```

Videos:
```
https://www.youtube.com/watch?v=DTHnCnkHxfk&ab_channel=JayMartMedia

https://www.youtube.com/watch?v=4zdBk6wisxc&ab_channel=AlexZiskind
```