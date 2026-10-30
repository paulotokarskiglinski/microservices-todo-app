FROM node:26-alpine AS build

WORKDIR /usr/local/app

COPY package.json package-lock.json ./
RUN npm ci --no-audit --no-fund

COPY ./ ./
RUN npm run build

FROM nginx:1.30-alpine

COPY --from=build /usr/local/app/dist/angular-todo-app/browser /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
