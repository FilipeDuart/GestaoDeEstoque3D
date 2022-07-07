const path = require('path');

module.exports = {
    entry: './Scripts/app.js',
    output: {
        path: path.resolve(__dirname, './Scripts/bundle'),
        filename: 'main.js',
        libraryTarget: 'var',
        library: 'Gestao'
    }
};