FROM node:16-alpine 

WORKDIR /app

COPY package*.json ./

RUN npm ci 

RUN npm install react-scripts@3.4.1 -g --silent

COPY . .

RUN npm run build

ENV NODE_ENV production

EXPOSE 3000

CMD [ "npx", "serve", "build" ]