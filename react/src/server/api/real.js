import express from 'express';
import modRewrite from 'connect-modrewrite';

const app = express();

const apiUrl = process.env.API_URL;
console.log(apiUrl)
app.use(modRewrite([
  `^/webapi/(.*)$ ${apiUrl}/$1 [P]`
]));

app.on('mount', () => {
  console.log('WebAPI proxy is available at /webapi');
});

export default app;
