/**
 * To-Do List Application with Local Storage
 * Features: Add, Edit, Delete, Filter, Search, Sort, Statistics, Export/Import
 */

class TodoApp {
    constructor() {
        this.todos = [];
        this.currentFilter = 'all';
        this.currentPriority = null;
        this.searchQuery = '';
        this.sortBy = 'newest';
        this.editingId = null;
        this.storageKey = 'todos_data';

        // DOM Elements
        this.todoInput = document.getElementById('todoInput');
        this.addBtn = document.getElementById('addBtn');
        this.todoList = document.getElementById('todoList');
        this.emptyState = document.getElementById('emptyState');
        this.searchInput = document.getElementById('searchInput');
        this.sortSelect = document.getElementById('sortSelect');
        this.filterTabs = document.querySelectorAll('.tab-btn');
        this.quickBtns = document.querySelectorAll('.quick-btn');
        this.clearCompletedBtn = document.getElementById('clearCompletedBtn');
        this.clearAllBtn = document.getElementById('clearAllBtn');
        this.exportBtn = document.getElementById('exportBtn');
        this.importBtn = document.getElementById('importBtn');
        this.fileInput = document.getElementById('fileInput');
        this.editModal = document.getElementById('editModal');
        this.closeModal = document.getElementById('closeModal');
        this.cancelEditBtn = document.getElementById('cancelEditBtn');
        this.saveEditBtn = document.getElementById('saveEditBtn');
        this.editInput = document.getElementById('editInput');
        this.editDescription = document.getElementById('editDescription');
        this.editPriority = document.getElementById('editPriority');
        this.editDeadline = document.getElementById('editDeadline');
        this.toast = document.getElementById('toast');

        this.init();
    }

    init() {
        this.loadFromStorage();
        this.setupEventListeners();
        this.render();
        this.updateStats();
    }

    setupEventListeners() {
        // Add Todo
        this.addBtn.addEventListener('click', () => this.handleAddTodo());
        this.todoInput.addEventListener('keypress', (e) => {
            if (e.key === 'Enter') this.handleAddTodo();
        });

        // Quick Priority Buttons
        this.quickBtns.forEach(btn => {
            btn.addEventListener('click', () => {
                const priority = btn.dataset.priority;
                this.todoInput.focus();
                this.currentPriority = priority;
            });
        });

        // Filter Tabs
        this.filterTabs.forEach(tab => {
            tab.addEventListener('click', () => {
                this.filterTabs.forEach(t => t.classList.remove('active'));
                tab.classList.add('active');
                this.currentFilter = tab.dataset.filter;
                this.render();
            });
        });

        // Search & Sort
        this.searchInput.addEventListener('input', (e) => {
            this.searchQuery = e.target.value.toLowerCase();
            this.render();
        });

        this.sortSelect.addEventListener('change', (e) => {
            this.sortBy = e.target.value;
            this.render();
        });

        // Clear Actions
        this.clearCompletedBtn.addEventListener('click', () => this.clearCompleted());
        this.clearAllBtn.addEventListener('click', () => this.clearAll());
        this.exportBtn.addEventListener('click', () => this.exportData());
        this.importBtn.addEventListener('click', () => this.fileInput.click());
        this.fileInput.addEventListener('change', (e) => this.importData(e));

        // Modal Controls
        this.closeModal.addEventListener('click', () => this.closeEditModal());
        this.cancelEditBtn.addEventListener('click', () => this.closeEditModal());
        this.saveEditBtn.addEventListener('click', () => this.saveEdit());
        this.editModal.addEventListener('click', (e) => {
            if (e.target === this.editModal) this.closeEditModal();
        });
    }

    handleAddTodo() {
        const text = this.todoInput.value.trim();
        if (!text) {
            this.showToast('Vui lòng nhập công việc', 'warning');
            return;
        }

        const todo = {
            id: Date.now(),
            text: text,
            description: '',
            completed: false,
            priority: this.currentPriority || 'medium',
            deadline: null,
            createdAt: new Date().toISOString(),
        };

        this.todos.unshift(todo);
        this.currentPriority = null;
        this.todoInput.value = '';
        this.saveToStorage();
        this.render();
        this.updateStats();
        this.showToast('✓ Công việc đã được thêm', 'success');
    }

    toggleTodo(id) {
        const todo = this.todos.find(t => t.id === id);
        if (todo) {
            todo.completed = !todo.completed;
            this.saveToStorage();
            this.render();
            this.updateStats();
        }
    }

    deleteTodo(id) {
        if (confirm('Bạn có chắc muốn xóa công việc này?')) {
            this.todos = this.todos.filter(t => t.id !== id);
            this.saveToStorage();
            this.render();
            this.updateStats();
            this.showToast('✓ Công việc đã bị xóa', 'success');
        }
    }

    openEditModal(id) {
        const todo = this.todos.find(t => t.id === id);
        if (!todo) return;

        this.editingId = id;
        this.editInput.value = todo.text;
        this.editDescription.value = todo.description || '';
        this.editPriority.value = todo.priority || 'medium';
        this.editDeadline.value = todo.deadline || '';
        this.editModal.classList.add('active');
    }

    closeEditModal() {
        this.editModal.classList.remove('active');
        this.editingId = null;
    }

    saveEdit() {
        const todo = this.todos.find(t => t.id === this.editingId);
        if (!todo) return;

        const text = this.editInput.value.trim();
        if (!text) {
            this.showToast('Tiêu đề không được để trống', 'warning');
            return;
        }

        todo.text = text;
        todo.description = this.editDescription.value;
        todo.priority = this.editPriority.value;
        todo.deadline = this.editDeadline.value;

        this.saveToStorage();
        this.render();
        this.updateStats();
        this.closeEditModal();
        this.showToast('✓ Công việc đã được cập nhật', 'success');
    }

    getFilteredTodos() {
        let filtered = this.todos;

        // Filter by status/priority
        if (this.currentFilter === 'active') {
            filtered = filtered.filter(t => !t.completed);
        } else if (this.currentFilter === 'completed') {
            filtered = filtered.filter(t => t.completed);
        } else if (this.currentFilter === 'high') {
            filtered = filtered.filter(t => t.priority === 'high');
        }

        // Search
        if (this.searchQuery) {
            filtered = filtered.filter(t =>
                t.text.toLowerCase().includes(this.searchQuery) ||
                t.description.toLowerCase().includes(this.searchQuery)
            );
        }

        // Sort
        filtered.sort((a, b) => {
            if (this.sortBy === 'newest') {
                return new Date(b.createdAt) - new Date(a.createdAt);
            } else if (this.sortBy === 'oldest') {
                return new Date(a.createdAt) - new Date(b.createdAt);
            } else if (this.sortBy === 'priority') {
                const priorityOrder = { high: 0, medium: 1, low: 2 };
                return priorityOrder[a.priority] - priorityOrder[b.priority];
            } else if (this.sortBy === 'name') {
                return a.text.localeCompare(b.text, 'vi');
            }
        });

        return filtered;
    }

    render() {
        const filtered = this.getFilteredTodos();

        if (filtered.length === 0) {
            this.todoList.innerHTML = '';
            this.emptyState.style.display = 'flex';
            return;
        }

        this.emptyState.style.display = 'none';
        this.todoList.innerHTML = filtered.map(todo => this.createTodoElement(todo)).join('');

        // Attach event listeners to rendered elements
        document.querySelectorAll('.todo-checkbox').forEach(checkbox => {
            checkbox.addEventListener('change', (e) => {
                this.toggleTodo(parseInt(e.target.dataset.id));
            });
        });

        document.querySelectorAll('.edit-btn').forEach(btn => {
            btn.addEventListener('click', (e) => {
                this.openEditModal(parseInt(e.currentTarget.dataset.id));
            });
        });

        document.querySelectorAll('.delete-btn').forEach(btn => {
            btn.addEventListener('click', (e) => {
                this.deleteTodo(parseInt(e.currentTarget.dataset.id));
            });
        });
    }

    createTodoElement(todo) {
        const createdDate = new Date(todo.createdAt).toLocaleDateString('vi-VN');
        const isOverdue = todo.deadline && new Date(todo.deadline) < new Date() && !todo.completed;

        return `
            <li class="todo-item priority-${todo.priority} ${todo.completed ? 'completed' : ''}">
                <input 
                    type="checkbox" 
                    class="todo-checkbox" 
                    ${todo.completed ? 'checked' : ''}
                    data-id="${todo.id}"
                >
                <div class="todo-content">
                    <div class="todo-text">${this.escapeHtml(todo.text)}</div>
                    ${todo.description ? `<div class="todo-description">${this.escapeHtml(todo.description)}</div>` : ''}
                    <div class="todo-meta">
                        <span class="priority-badge priority-${todo.priority}">
                            <i class="fa ${this.getPriorityIcon(todo.priority)}"></i>
                            ${this.getPriorityLabel(todo.priority)}
                        </span>
                        ${todo.deadline ? `
                            <span class="deadline-badge ${isOverdue ? 'overdue' : ''}">
                                <i class="fa fa-calendar"></i>
                                ${new Date(todo.deadline).toLocaleDateString('vi-VN')}
                            </span>
                        ` : ''}
                        <span class="created-badge">
                            <i class="fa fa-clock-o"></i>
                            ${createdDate}
                        </span>
                    </div>
                </div>
                <div class="todo-actions">
                    <button class="todo-action-btn edit-btn" data-id="${todo.id}" title="Chỉnh sửa">
                        <i class="fa fa-edit"></i>
                    </button>
                    <button class="todo-action-btn delete-btn" data-id="${todo.id}" title="Xóa">
                        <i class="fa fa-trash"></i>
                    </button>
                </div>
            </li>
        `;
    }

    getPriorityIcon(priority) {
        switch (priority) {
            case 'high': return 'fa-exclamation-circle';
            case 'medium': return 'fa-circle';
            case 'low': return 'fa-circle-o';
            default: return 'fa-circle';
        }
    }

    getPriorityLabel(priority) {
        switch (priority) {
            case 'high': return 'Ưu tiên cao';
            case 'medium': return 'Bình thường';
            case 'low': return 'Ưu tiên thấp';
            default: return 'Bình thường';
        }
    }

    updateStats() {
        const total = this.todos.length;
        const completed = this.todos.filter(t => t.completed).length;
        const priority = this.todos.filter(t => t.priority === 'high').length;
        const percent = total === 0 ? 0 : Math.round((completed / total) * 100);

        document.getElementById('totalStats').textContent = total;
        document.getElementById('completedStats').textContent = completed;
        document.getElementById('priorityStats').textContent = priority;
        document.getElementById('percentStats').textContent = `${percent}%`;

        // Update badges
        document.getElementById('activeBadge').textContent = total - completed;
        document.getElementById('completedBadge').textContent = completed;
    }

    clearCompleted() {
        if (confirm('Xóa tất cả công việc đã hoàn thành?')) {
            this.todos = this.todos.filter(t => !t.completed);
            this.saveToStorage();
            this.render();
            this.updateStats();
            this.showToast('✓ Đã xóa công việc hoàn thành', 'success');
        }
    }

    clearAll() {
        if (confirm('⚠️ Bạn có chắc muốn xóa TOÀN BỘ công việc? Hành động này không thể hoàn tác!')) {
            if (confirm('Xác nhận lần cuối: Xóa tất cả?')) {
                this.todos = [];
                this.saveToStorage();
                this.render();
                this.updateStats();
                this.showToast('✓ Đã xóa tất cả công việc', 'success');
            }
        }
    }

    exportData() {
        const data = {
            exportDate: new Date().toISOString(),
            todos: this.todos
        };

        const json = JSON.stringify(data, null, 2);
        const blob = new Blob([json], { type: 'application/json' });
        const url = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `todos_${new Date().toISOString().split('T')[0]}.json`;
        link.click();
        URL.revokeObjectURL(url);

        this.showToast('✓ Dữ liệu đã được xuất', 'success');
    }

    importData(event) {
        const file = event.target.files[0];
        if (!file) return;

        const reader = new FileReader();
        reader.onload = (e) => {
            try {
                const data = JSON.parse(e.target.result);
                if (!data.todos || !Array.isArray(data.todos)) {
                    throw new Error('Định dạng file không hợp lệ');
                }

                if (confirm('Nhập dữ liệu sẽ thay thế toàn bộ công việc hiện tại. Tiếp tục?')) {
                    this.todos = data.todos;
                    this.saveToStorage();
                    this.render();
                    this.updateStats();
                    this.showToast('✓ Dữ liệu đã được nhập', 'success');
                }
            } catch (error) {
                this.showToast('❌ Lỗi: ' + error.message, 'error');
            }
        };
        reader.readAsText(file);
        this.fileInput.value = '';
    }

    saveToStorage() {
        try {
            localStorage.setItem(this.storageKey, JSON.stringify(this.todos));
        } catch (error) {
            this.showToast('❌ Lỗi lưu dữ liệu: ' + error.message, 'error');
        }
    }

    loadFromStorage() {
        try {
            const stored = localStorage.getItem(this.storageKey);
            this.todos = stored ? JSON.parse(stored) : [];
        } catch (error) {
            console.error('Lỗi tải dữ liệu:', error);
            this.todos = [];
        }
    }

    showToast(message, type = 'info') {
        this.toast.textContent = message;
        this.toast.className = `toast show ${type}`;
        setTimeout(() => {
            this.toast.classList.remove('show');
        }, 3000);
    }

    escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }
}

// Initialize app when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        new TodoApp();
    });
} else {
    new TodoApp();
}
