import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { map, tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

interface LoginResponse { token: string; }

@Injectable({ providedIn: 'root' })
export class AuthService {
    private readonly tokenKey = 'token';
    private _role: string | null = null;

    constructor(
        private http: HttpClient,
        private router: Router
    ) { }

    login(credentials: { username: string; password: string; }): Observable<void> {
        return this.http.post<LoginResponse>(
            'http://localhost:5294/api/auth/login',
            credentials
        ).pipe(
            tap(res => {
                localStorage.setItem(this.tokenKey, res.token);
                this._role = this.extractRole(res.token);
            }),
            map(() => { })
        );
    }

    logout(): void {
        localStorage.removeItem(this.tokenKey);
        this._role = null;
        this.router.navigate(['/login']);
    }

    get isLoggedIn(): boolean {
        return !!localStorage.getItem(this.tokenKey);
    }

    get token(): string | null {
        return localStorage.getItem(this.tokenKey);
    }

    get role(): string | null {
        if (this._role) {
            return this._role;
        }
        const tok = localStorage.getItem(this.tokenKey);
        if (!tok) {
            return null;
        }
        this._role = this.extractRole(tok);
        return this._role;
    }

    private extractRole(token: string): string | null {
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            return payload['role']
                || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
                || null;
        } catch {
            return null;
        }
    }
}