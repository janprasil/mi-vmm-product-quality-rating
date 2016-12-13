import express from 'express';
import modRewrite from 'connect-modrewrite';

const app = express();

const apiUrl = 'http://f2062087.ngrok.io/api';

app.use(modRewrite([
  `^/webapi/(.*)$ ${apiUrl}/$1 [P]`
]));

app.on('mount', () => {
  console.log('WebAPI proxy is available at /webapi');
});

export default app;
