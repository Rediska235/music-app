FROM node:16-alpine 

WORKDIR /app

COPY MusicApp.ReactClient/react-client/package*.json ./

RUN npm ci 

COPY MusicApp.ReactClient/react-client/ .

RUN npm run build

ENV NODE_ENV production

EXPOSE 3000

CMD [ "npx", "serve", "build" ]