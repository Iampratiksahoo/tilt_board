const express = require('express');
const app = express();
const PORT = 3000;

app.get('/', (req, res) => {
  res.send('Tilt Board Server Running');
});

app.listen(PORT, () => {
  console.log(`REST API listening at http://localhost:${PORT}`);
});

