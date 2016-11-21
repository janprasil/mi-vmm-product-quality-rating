/* eslint-disable quotes */
import express from 'express';

const app = express();

app.post('/contours', (req, res) => {
  console.log(req)
  res.json({ ahoj: 'sdfsdf' });
});

app.on('mount', () => {
  console.log('API is at %s', app.mountpath);
});

export default app;
